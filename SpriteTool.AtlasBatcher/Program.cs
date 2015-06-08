// <copyright file="Program.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AtlasBatcher.Program class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.AtlasBatcher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Atom.Diagnostics;
    using Atom.Storage;
    using Atom.Tools.SpriteTool.Atlas;
    using Atom.Tools.SpriteTool.Database;
    using Atom.Xna;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Sprite Atlas Batcher is a simple batch program that uses XML-configuration files to generate an Atom Sprite Database (.sdb) given a set of sprite images.
    /// Additionally the individual sprites are compacted into a single sprite atlas texture. As such only one texture must be loaded into video memory.
    /// The Sprite Database contains information such as in what location each indivual sprite is located in the sprite atlas.
    /// The Sprite Database can also contain Sprite Animations. These are created using the Sprite Tool.
    /// </summary>
    public sealed class Program : Atom.Xna.Wpf.HiddenXnaGame
    {
        private const string ConfigFolder = @"Content/Configuration/";
        private const string ConfigExtension = ".xml";

        public static void Main( string[] args )
        {
            Console.WriteLine( "Sprite Atlas Batcher" );

            if( args.Length == 0 )
            {
                Console.WriteLine();
                Console.WriteLine( "Enter the name of configuration file to batch:" );

                string path = Console.ReadLine();
                Console.WriteLine();

                if( !path.StartsWith( ConfigFolder, StringComparison.Ordinal ) )
                {
                    path = ConfigFolder + path;
                }

                if( !path.EndsWith( ConfigExtension ) )
                {
                    path += ConfigExtension;
                }

                args = new string[] { 
                   path
                };
            }
            else
            {
                foreach( var arg in args )
                {
                    Console.Write( "    " );
                    Console.WriteLine( arg );
                }
            }

            Console.WriteLine();

            using( var program = new Program( args ) )
            {
                program.Run();
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Initializes a new instance of the Program class.
        /// </summary>
        /// <param name="args">
        /// Stores the arguments that have been passed to the Program.
        /// </param>
        private Program( string[] args )
        {
            this.Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.args = args;
        }

        /// <summary>
        /// Called when the Xna framework is preparing the GraphicsDevice.
        /// </summary>
        /// <param name="e">
        /// The <see cref="PreparingDeviceSettingsEventArgs"/> that contain the event data.
        /// </param>
        protected override void OnPreparingDeviceSettings( Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs e )
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            e.GraphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.None;
        }

        /// <summary>
        /// Called after initialization has been completed;
        /// but before any Update call.
        /// </summary>
        protected override void BeginRun()
        {
            foreach( string arg in args )
            {
                try
                {
                    log.WriteLine( "\n{0}:", arg );
                    log.WriteLine( "----------------------------" );

                    AtlasConfiguration configuration = LoadConfiguration( arg );
                    IEnumerable<Texture2D> images = LoadSpriteImages( configuration );
                    TextureAtlas atlas = BuildSpriteAtlas( configuration, images );

                    RenderedTextureAtlas renderedTextureAtlas = RenderTextureAtlas( configuration, atlas );
                    SaveTextureAtlas( configuration, renderedTextureAtlas );

                    SpriteAtlas spriteAtlas = BuildSpriteAtlas( renderedTextureAtlas );
                    SaveSpriteAtlas( configuration, spriteAtlas );

                    log.WriteLine( "\n----------------------------\n" );
                }
                catch( Exception exc )
                {
                    log.WriteLine();
                    log.WriteLine( exc.ToString() );
                    log.WriteLine();
                    log.WriteLine();
                }
            }

            this.Exit();
        }

        private void SaveSpriteAtlas( AtlasConfiguration configuration, SpriteAtlas spriteAtlas )
        {
            if( configuration.SpriteOutputType == AtlasSpriteOutputType.Database )
            {
                string databaseName = Path.GetFileNameWithoutExtension( configuration.OutputTextureName );
                string databaseFileName = databaseName + SpriteDatabase.ReaderWriter.Extension;
                string databasePath = Path.Combine( configuration.SpriteOutputFolder, databaseFileName );

                var database = new SpriteDatabase() {
                    Name = databaseName,
                    Atlas = spriteAtlas,
                };

                var textureLoader = new UncachedFromFileTexture2DLoader( this.Graphics );
                var databaseSaver = new SpriteDatabaseSaver( log, textureLoader );
                databaseSaver.Save( database, databasePath, configuration.PreserveSpriteProperties );

                log.WriteLine( "Database saved to {0}.", databasePath );
            }
        }

        private static SpriteAtlas BuildSpriteAtlas( RenderedTextureAtlas renderedTextureAtlas )
        {
            var spriteAtlasBuilder = new SpriteAtlasBuilder();
            SpriteAtlas spriteAtlas = spriteAtlasBuilder.Build( renderedTextureAtlas );
            return spriteAtlas;
        }

        private void SaveTextureAtlas( AtlasConfiguration configuration, RenderedTextureAtlas renderedTextureAtlas )
        {
            Directory.CreateDirectory( Path.GetDirectoryName( configuration.OutputTextureName ) );

            using( var stream = File.Create( configuration.OutputTextureName ) )
            {
                Texture2D atlasTexture2 = renderedTextureAtlas.AtlasTexture;
                atlasTexture2.SaveAsPng( stream, atlasTexture2.Width, atlasTexture2.Height );
            }

            log.WriteLine( "Atlas Texture saved." );
        }

        private RenderedTextureAtlas RenderTextureAtlas( AtlasConfiguration configuration, TextureAtlas atlas )
        {
            var atlasRenderer = new AtlasRenderer( this.Graphics );
            Texture2D atlasTexture = atlasRenderer.Render( atlas, configuration );
            RenderedTextureAtlas renderedTextureAtlas = new RenderedTextureAtlas( atlas, atlasTexture, configuration );
            return renderedTextureAtlas;
        }

        private TextureAtlas BuildSpriteAtlas( AtlasConfiguration configuration, IEnumerable<Texture2D> images )
        {
            var atlasBuilder = new AtlasBuilder();

            TextureAtlas atlas;
            if( atlasBuilder.TryBuild( configuration.AtlasSize, configuration.Spacing, images, out atlas ) )
            {
                log.WriteLine( "Atlas built." );
            }
            else
            {
                log.WriteLine(
                    "Warning! Only {0} of {1} images could fit into the atlas. Press any key to continue.",
                    atlas.Entries.Count(),
                    images.Count()
                );
            }

            return atlas;
        }

        private IEnumerable<Texture2D> LoadSpriteImages( AtlasConfiguration configuration )
        {
            IEnumerable<Texture2D> images = new ImageFinder( this.Graphics ).FindRecursively( configuration.InputImageFolder );

            log.WriteLine();
            log.WriteLine( "Found {0} images.", images.Count() );
            return images;
        }

        private AtlasConfiguration LoadConfiguration( string arg )
        {
            AtlasConfiguration configuration = XmlUtilities.Deserialize<AtlasConfiguration>( arg );

            log.WriteLine( "{0,-35} - Input Directory", configuration.InputImageFolder );
            log.WriteLine( "{0,-35} - Output Texture", configuration.OutputTextureName );
            log.WriteLine( "{0,-35} - Size", configuration.AtlasSize.ToString() );
            log.WriteLine( "{0,-35} - Spacing", configuration.Spacing );
            log.WriteLine( "{0,-35} - Sprite Output Type", configuration.SpriteOutputType );
            log.WriteLine( "{0,-35} - Sprite Output Folder", configuration.SpriteOutputFolder );
            log.WriteLine( "{0,-35} - Preserve Sprite Properties", configuration.PreserveSpriteProperties );
            return configuration;
        }

        private readonly ILog log = new ConsoleLog();

        /// <summary>
        /// Stores the arguments that have been passed to the Program.
        /// </summary>
        private readonly string[] args;
    }
}
