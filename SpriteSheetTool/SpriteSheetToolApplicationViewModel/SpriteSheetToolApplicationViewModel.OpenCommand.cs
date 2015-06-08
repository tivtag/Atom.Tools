// <copyright file="SpriteSheetToolApplicationViewModel.OpenCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetToolApplicationViewModel.OpenCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using Atom.Storage;
    using Atom.Wpf;
    using Atom.Xna;
    using Microsoft.Win32;
    using System.IO;
    using System;

    /// <content>
    /// Contains the OpenCommand of the SpriteSheetToolApplicationViewModel class.
    /// </content>
    public partial class SpriteSheetToolApplicationViewModel
    {
        /// <summary>
        /// Defines the ICommand that when executed allows the
        /// user to open an existing SpriteSheet.
        /// </summary>
        private sealed class OpenCommand : ViewModelCommand<SpriteSheetToolApplicationViewModel, SpriteSheetToolApplication>
        {
            /// <summary>
            /// Initializes a new instance of the OpenCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteSheetToolApplicationViewModel that owns the new OpenCommand.
            /// </param>
            public OpenCommand( SpriteSheetToolApplicationViewModel viewModel )
                : base( viewModel )
            {
            }

            /// <summary>
            /// Executes this ICommand.
            /// </summary>
            /// <param name="parameter">
            /// The parameter passed to the ICommand.
            /// </param>
            public override void Execute( object parameter )
            {
                if( !this.CanExecute( parameter ) )
                    return;

                var dialog = new OpenFileDialog() {
                    Filter = "Sprite Sheets (*.sprsh)|*.sprsh",
                    InitialDirectory = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, @"Content\Sheets\" )
                };

                if( dialog.ShowDialog() == true )
                {
                    SafeExecute.WithMsgBox( () => {
                        var sheet = StorageUtilities.LoadFromFile<SpriteSheet>(
                            dialog.FileName,
                            new SpriteSheet.ReaderWriter( this.ViewModel.SpriteLoader )
                        );

                        this.ViewModel.SpriteSheet = new SpriteSheetViewModel( sheet, this.ViewModel.SpriteSource );
                    } );
                }
            }
        }
    }
}
