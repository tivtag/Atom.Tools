// <copyright file="ImageFinder.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.ImageFinder class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Atom.Xna;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a mechanism that finds and loads the images within a directory.
    /// </summary>
    public sealed class ImageFinder
    {
        private static readonly string[] ImageTypes = new string[] {
            ".tga",
            ".png",
            ".jpg",
            ".gif"
        };

        /// <summary>
        /// Initializes a new instance of the ImageFinder class.
        /// </summary>
        /// <param name="graphicsDeviceService">
        /// Provides a mechanism that allows to receive the Xna GraphicsDevice.
        /// </param>
        public ImageFinder( IGraphicsDeviceService graphicsDeviceService )
        {
            this.textureLoader = new UncachedTargaFromFileTexture2DLoader( graphicsDeviceService );
        }

        /// <summary>
        /// Returns all images that are the given directory and its sub directories.
        /// </summary>
        /// <param name="directory">
        /// The directory to process.
        /// </param>
        /// <returns>
        /// The images that have been loaden.
        /// </returns>
        public IEnumerable<Texture2D> FindRecursively( string directory )
        {
            string[] filesNames = this.GetImageFileNames( directory );
            return this.LoadImages( filesNames );
        }

        /// <summary>
        /// Gets the filesnames of all images in the given directory.
        /// </summary>
        /// <param name="directory">
        /// The directory to process.
        /// </param>
        /// <returns>
        /// The names of the image files.
        /// </returns>
        private string[] GetImageFileNames( string directory )
        {
            return (from file in Directory.EnumerateFiles( directory, "*", SearchOption.AllDirectories )
                    where FilterImageFileName( file )
                    select file).ToArray();
        }

        /// <summary>
        /// Gets a value indicating whether the specified file should be returned.
        /// </summary>
        /// <param name="file">
        /// The file to filter.
        /// </param>
        /// <returns>
        /// true if it should be included;
        /// -or- otherwise false.
        /// </returns>
        private bool FilterImageFileName( string file )
        {
            // TODO: Actually filter for image types.
            bool isSvnFile = file.Contains( ".svn", StringComparison.Ordinal );

            if( isSvnFile )
            {
                return false;
            }

            string extension = Path.GetExtension( file );
            return ImageTypes.Any( it => it.Equals( extension, StringComparison.OrdinalIgnoreCase ) );
        }

        /// <summary>
        /// Loads the given images.
        /// </summary>
        /// <param name="filesNames">
        /// The names of the image files to load.
        /// </param>
        /// <returns>
        /// The images that have been loaden.
        /// </returns>
        private IEnumerable<Texture2D> LoadImages( string[] filesNames )
        {
            List<Texture2D> textures = new List<Texture2D>();

            foreach( string fileName in filesNames )
            {
                try
                {
                    Texture2D texture = this.textureLoader.Load( fileName );
                    textures.Add( texture );
                }
                catch( Exception exc )
                {
                    System.Console.WriteLine( "Error loading image '{0}': {1}", fileName, exc.Message );
                }
            }

            return textures;
        }

        /// <summary>
        /// Provides a mechanism for loading the image files into a texture.
        /// </summary>
        private readonly Atom.Xna.ITexture2DLoader textureLoader;
    }
}
