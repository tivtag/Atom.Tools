
namespace TileExtractionTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Atom.Diagnostics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Atom.Collections;

    /// <summary>
    /// The TileExtraction Tool is used to extract individual image files from a single tilesheet.
    /// </summary>
    public sealed class Program : Atom.Xna.Wpf.HiddenXnaGame
    {
        static void Main( string[] args )
        {
            Console.WriteLine();

            using( var program = new Program() )
            {
                program.Run();
            }

            Console.ReadKey();
        }
         
        /// <summary>
        /// Called after initialization has been completed;
        /// but before any Update call.
        /// </summary>
        protected override void BeginRun()
        {
            try
            {
                Console.WriteLine( "Enter input file name:" );
                string inputFileName = Console.ReadLine();

                // Console.WriteLine( "Enter output tile size:" );
                int outputTileSize = 16;
                bool transformAlpha = true;

                // Load
                Texture2D texture = LoadTexture( inputFileName );
                List<ExtractedTile> tiles = LoadTiles( outputTileSize, texture );

                // Minimize
                RemoveDuplicates( tiles );

                // Output
                OutputTiles( inputFileName, outputTileSize, transformAlpha, tiles );
            }
            catch( Exception exc )
            {
                log.WriteLine();
                log.WriteLine( exc.ToString() );
                log.WriteLine();
                log.WriteLine();
            }

            this.Exit();
        }

        private Texture2D LoadTexture( string inputFileName )
        {
            Texture2D texture = null;
            using( var stream = File.OpenRead( inputFileName ) )
            {
                texture = Texture2D.FromStream( this.GraphicsDevice, stream );
            }

            Console.WriteLine( "Texture loaded.." );
            return texture;
        }

        private List<ExtractedTile> LoadTiles( int outputTileSize, Texture2D texture )
        {
            var tiles = new List<ExtractedTile>();

            for( int x = 0; x <= texture.Width - outputTileSize; x += outputTileSize )
            {
                for( int y = 0; y <= texture.Height - outputTileSize; y += outputTileSize )
                {
                    Color[] data = new Color[outputTileSize * outputTileSize];
                    texture.GetData<Color>( 0, new Microsoft.Xna.Framework.Rectangle( x, y, outputTileSize, outputTileSize ), data, 0, data.Length );

                    tiles.Add( new ExtractedTile() { Data = data, X = x, Y = y } );
                }
            }

            log.WriteLine( "{0} tiles in total", tiles.Count );
            return tiles;
        }

        private void RemoveDuplicates( List<ExtractedTile> tiles )
        {
            var removedTiles = new List<ExtractedTile>();

            for( int i = 0; i < tiles.Count; ++i )
            {
                var tile = tiles[i];

                for( int j = 0; j < tiles.Count; ++j )
                {
                    if( i != j )
                    {
                        var otherTile = tiles[j];

                        if( tile.MatchesData( otherTile ) )
                        {
                            tiles.RemoveAt( i );
                            removedTiles.Add( tile );
                            --i;
                            break;
                        }
                    }
                }
            }

            log.WriteLine( "{0} duplicates removed", removedTiles.Count );
        }

        private void OutputTiles( string inputFileName, int outputTileSize, bool transformAlpha, List<ExtractedTile> tiles )
        {
            string outputDir = "Output";
            Directory.CreateDirectory( outputDir );

            foreach( ExtractedTile tile in tiles )
            {
                var tileTexture = new Texture2D( this.GraphicsDevice, outputTileSize, outputTileSize );

                if( transformAlpha )
                {
                    for( int i = 0; i < tile.Data.Length; ++i )
                    {
                        if( tile.Data[i] == Color.White )
                        {
                            tile.Data[i] = Color.Transparent;
                        }
                    }
                }

                tileTexture.SetData( tile.Data );

                string outputFileName = Path.Combine( outputDir, string.Format( "{0}_{1}x{2}.png", Path.GetFileNameWithoutExtension( inputFileName ), tile.X, tile.Y ) );
                using( var stream = File.Create( outputFileName ) )
                {
                    tileTexture.SaveAsPng( stream, outputTileSize, outputTileSize );
                }
            }
        }

        private readonly ILog log = new ConsoleLog();
    }
}
