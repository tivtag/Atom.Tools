// <copyright file="SpriteToolApplicationViewModel.ImportAnimatedSpritesCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteToolApplicationViewModel.ImportAnimatedSpritesCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using Atom.Wpf;
    using System.Linq;
    using System.IO;
    using Atom.Storage;
    using Atom.Tools.SpriteTool.Import;
    using System.Collections.Generic;
    using Atom.Xna;
    using System;
    using System.Windows;

    /// <content>
    /// Contains the ImportAnimatedSpritesCommand of the SpriteToolApplicationViewModel class.
    /// </content>
    public partial class SpriteToolApplicationViewModel
    {
        /// <summary>
        /// Defines an ICommand that allows the user to import Animated Sprites that fit into the 
        /// current SpriteDatabase.
        /// </summary>
        private sealed class ImportAnimatedSpritesCommand : ViewModelCommand<SpriteToolApplicationViewModel, SpriteToolApplication>
        {
            /// <summary>
            /// Initializes a new instance of the ImportAnimatedSpritesCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteToolApplicationViewModel that owns the new ImportAnimatedSpritesCommand.
            /// </param>
            public ImportAnimatedSpritesCommand( SpriteToolApplicationViewModel viewModel )
                : base( viewModel )
            {
                viewModel.SpriteDatabaseChanged += ( sender, e ) => this.OnCanExecuteChanged();
            }

            /// <summary>
            /// Gets a value indicating whether this ICommand can
            /// currently be executed.
            /// </summary>
            /// <param name="parameter">
            /// The paramter passed to the command.
            /// </param>
            /// <returns>
            /// true if it can be executed;
            /// otherwise false.
            /// </returns>
            public override bool CanExecute( object parameter )
            {
                return this.ViewModel.SpriteDatabase != null;
            }

            /// <summary>
            /// Executes this ICommand.
            /// </summary>
            /// <param name="parameter">
            /// The paramter passed to the command.
            /// </param>
            public override void Execute( object parameter )
            {
                if( !this.CanExecute( parameter ) )
                    return;

                var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

                if( dialog.ShowDialog() == true )
                {
                    var files = GetMatchingFiles( dialog.SelectedPath );
                    var sprites = GetAllAnimatedSprites( files );
                    int importCount = 0;
                    
                    foreach( var sprite in sprites )
                    {
                        if( this.ShouldImport( sprite ) )
                        {
                            this.Import( sprite );
                            ++importCount;
                        }
                    }

                    MessageBox.Show(
                        string.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
                            "{0} animates sprites imported.",
                            importCount.ToString( System.Globalization.CultureInfo.CurrentCulture )
                        ),
                        string.Empty,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }

            /// <summary>
            /// Gets the names of all files that might fit for importing.
            /// </summary>
            /// <param name="path">
            /// The path to search.
            /// </param>
            /// <returns>
            /// The names of fitting files within the given directory.
            /// </returns>
            private static string[] GetMatchingFiles( string path )
            {
                return Directory.GetFiles( path, "*.aspr", SearchOption.AllDirectories );
            }

            /// <summary>
            /// Gets all AnimatedSpriteData that can be loaded from the given files.
            /// </summary>
            /// <param name="files">
            /// The names of the files to load.
            /// </param>
            /// <returns>
            /// The AnimatedSpriteData that could be loaded
            /// from the given files.
            /// </returns>
            private static IEnumerable<AnimatedSpriteData> GetAllAnimatedSprites( string[] files )
            {
                foreach( string file in files )
                {
                    AnimatedSpriteData spriteData = null;

                    try
                    {
                       spriteData = XmlUtilities.Deserialize<AnimatedSpriteData>( file );                            
                    }
                    catch {
                        // eat it!
                    }

                    if( spriteData != null )
                    {
                        yield return spriteData;
                    }
                }
            }


            /// <summary>
            /// Gets a value indicating whether the given AnimatedSprite should
            /// be imported into the current SpriteDatabase.
            /// </summary>
            /// <param name="animatedSprite">
            /// The data descriping the AnimatedSprite to import.
            /// </param>
            /// <returns>
            /// true if it should be imported;
            /// otherwise false.
            /// </returns>
            private bool ShouldImport( AnimatedSpriteData animatedSprite )
            {
                var spriteDatabase = this.ViewModel.SpriteDatabase.Model;
                
                if( spriteDatabase.AnimatedSprites.Any(
                    ( spr ) => spr.Name.Equals( animatedSprite.Name, StringComparison.Ordinal ) ) )
                {
                    return false;
                }

                foreach( var frame in animatedSprite.Frames )
                {
                    if( frame.SpriteName.Length > 0 )
                    {
                        var sprite = spriteDatabase.FindSprite( frame.SpriteName );
                        if( sprite == null )
                            return false;
                    }
                }

                return true;
            }

            /// <summary>
            /// Imports the given AnimatedSprite into the current SpriteDatabase.
            /// </summary>
            /// <param name="animatedSpriteData">
            /// The AnimatedSpriteData that descripes the AnimatedSprite to import.
            /// </param>
            private void Import( AnimatedSpriteData animatedSpriteData )
            {              
                int frameCount = animatedSpriteData.Frames.Length;
                var spriteDatabaseViewModel = this.ViewModel.SpriteDatabase;
                var spriteDatabase = spriteDatabaseViewModel.Model;  
               
                var animatedSprite = AnimatedSprite.CreateManual(
                    animatedSpriteData.Name,
                    frameCount,
                    animatedSpriteData.DefaultAnimationSpeed,
                    animatedSpriteData.IsLoopingByDefault
                );

                for( int i = 0; i < frameCount; ++i )
                {
                    AnimatedSpriteFrame frame = animatedSprite[i];
                    AnimatedSpriteFrameData frameData = animatedSpriteData.Frames[i];

                    frame.Offset = frameData.Offset;
                    frame.Time = frameData.Time;

                    if( frameData.SpriteName.Length > 0 )
                    {
                        frame.Sprite = spriteDatabase.FindSprite( frameData.SpriteName );
                    }
                }

                spriteDatabaseViewModel.AddAnimatedSprite( animatedSprite );
            }
        }
    }
}
