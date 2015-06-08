// <copyright file="SpriteToolApplicationViewModel.AddNewAnimatedSpriteCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteToolApplicationViewModel.AddNewAnimatedSpriteCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Atom.Tools.SpriteTool.Properties;
    using Atom.Wpf;
    using Microsoft.Win32;
    using Microsoft.Xna.Framework.Graphics;
    using System.Linq;
    using Atom.Collections;

    /// <content>
    /// Contains the ExtractTilesCommand of the SpriteToolApplicationViewModel class.
    /// </content>
    public partial class SpriteToolApplicationViewModel
    {
        /// <summary>
        /// Defines an ICommand that when executed adds a new -empty- AnimatedSprite to the SpriteDatabase.
        /// </summary>
        private sealed class ExtractTilesCommand : ViewModelCommand<SpriteToolApplicationViewModel, SpriteToolApplication>
        {
            private const int TileSize = 16;
            private const int TilePixelStride = 4 * TileSize;

            /// <summary>
            /// Initializes a new instance of the ExtractTilesCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteToolApplicationViewModel that owns the new ExtractTilesCommand.
            /// </param>
            public ExtractTilesCommand( SpriteToolApplicationViewModel viewModel )
                : base( viewModel )
            {
            }

            /// <summary>
            /// Executes this ICommand.
            /// </summary>
            /// <param name="parameter">
            /// The paramter passed to the command.
            /// </param>
            public override void Execute( object parameter )
            {
                OpenFileDialog dialog = new OpenFileDialog() {
                    Filter = Resources.FilterImageFiles
                };

                if( dialog.ShowDialog() == true )
                {
                    try
                    {
                        Extract( dialog.FileName );
                    }
                    catch( System.Exception ex )
                    {
                        MessageBox.Show( ex.ToString(), "An exception has occurred. Sorry!", MessageBoxButton.OK, MessageBoxImage.Error );
                    }
                }
            }

            private void Extract( string filePath )
            {
                string sheetName =  Path.GetFileNameWithoutExtension( filePath );
                string path = Path.Combine( Path.GetDirectoryName( filePath ), sheetName );
                Directory.CreateDirectory( path );

                BitmapImage bitmap = new BitmapImage( new Uri( filePath ) );
                List<int[]> tiles = new List<int[]>();

                for( int row = 0; row < bitmap.PixelHeight / TileSize; ++row)
                {
                    for( int column = 0; column < bitmap.PixelWidth / TileSize; ++column )
                    {
                        int[] pixels = new int[TileSize * TileSize];
                        bitmap.CopyPixels( new Int32Rect( column * TileSize, row * TileSize, TileSize, TileSize ), pixels, TilePixelStride, 0 );

                        if( !tiles.Any( p => p.SequenceEqual( pixels ) ) )
                        {
                            string outputFilename = Path.Combine( path, sheetName + "_" + tiles.Count + ".tga" );
                            SaveOutput( outputFilename, pixels );
                            tiles.Add( pixels );
                        }
                    }                    
                }
            }

            private static void SaveOutput( string filePath, int[] pixels )
            {
                var outputBitmap = new WriteableBitmap( 16, 16, 96.0, 96.0, PixelFormats.Pbgra32, null );
                outputBitmap.WritePixels( new Int32Rect( 0, 0, TileSize, TileSize ), pixels, TilePixelStride, 0 );

                using( var stream = File.Create( filePath ) )
                {
                    TgaWriter.Write( outputBitmap, stream );
                }
            }

            public static class TgaWriter
            {
                public static void Write( WriteableBitmap bitmap, Stream output )
                {
                    int width = bitmap.PixelWidth;
                    int height = bitmap.PixelHeight;
                    int[] pixels = new int[width * height];
                    int widthInBytes = 4 * width;
                    bitmap.CopyPixels( pixels, widthInBytes, 0 );

                    byte[] pixelsArr = new byte[pixels.Length * 4];

                    int offsetSource = 0;
                    int width4 = width * 4;
                    int width8 = width * 8;
                    int offsetDest = (height - 1) * width4;
                    for( int y = 0; y < height; y++ )
                    {
                        for( int x = 0; x < width; x++ )
                        {
                            int value = pixels[offsetSource];
                            pixelsArr[offsetDest] = (byte)(value & 255); // b
                            pixelsArr[offsetDest + 1] = (byte)((value >> 8) & 255); // g
                            pixelsArr[offsetDest + 2] = (byte)((value >> 16) & 255); // r
                            pixelsArr[offsetDest + 3] = (byte)(value >> 24); // a

                            offsetSource++;
                            offsetDest += 4;
                        }
                        offsetDest -= width8;
                    }

                    byte[] header = new byte[] {
                        0, // ID length
                        0, // no color map
                        2, // uncompressed, true color
                        0, 0, 0, 0,
                        0,
                        0, 0, 0, 0, // x and y origin
                        (byte)(width & 0x00FF),
                        (byte)((width & 0xFF00) >> 8),
                        (byte)(height & 0x00FF),
                        (byte)((height & 0xFF00) >> 8),
                        32, // 32 bit bitmap
                        0 };

                    using( BinaryWriter writer = new BinaryWriter( output ) )
                    {
                        writer.Write( header );
                        writer.Write( pixelsArr );
                    }
                }
            }
        }
    }
}
