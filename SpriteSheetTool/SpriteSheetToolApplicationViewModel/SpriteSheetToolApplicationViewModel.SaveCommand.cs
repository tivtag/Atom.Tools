// <copyright file="SpriteSheetToolApplicationViewModel.SaveCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetToolApplicationViewModel.SaveCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System;
    using System.IO;
    using System.Windows;
    using Atom.Storage;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Contains the SaveCommand of the SpriteSheetToolApplicationViewModel class.
    /// </content>
    public partial class SpriteSheetToolApplicationViewModel
    {
        /// <summary>
        /// Defines the ICommand that when executed allows saves
        /// the current SpriteSheet.
        /// </summary>
        private sealed class SaveCommand : ViewModelCommand<SpriteSheetToolApplicationViewModel, SpriteSheetToolApplication>
        {
            /// <summary>
            /// Initializes a new instance of the SaveCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteSheetToolApplicationViewModel that owns the new ImportCommand.
            /// </param>
            public SaveCommand( SpriteSheetToolApplicationViewModel viewModel )
                : base( viewModel )
            {
                viewModel.SpriteSheetChanged += (sender, e) => this.OnCanExecuteChanged();
            }

            /// <summary>
            /// Gets a value indicating whether this ICommand can currently
            /// be executed.
            /// </summary>
            /// <param name="parameter">
            /// The parameter passed to the ICommand.
            /// </param>
            /// <returns>
            /// true if it can be executed;
            /// otherwise false.
            /// </returns>
            public override bool CanExecute( object parameter )
            {
                return this.ViewModel.SpriteSheet != null;
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

                SafeExecute.WithMsgBox( () => {

                    string path = this.GetSavePath();
                    Directory.CreateDirectory( Path.GetDirectoryName( path ) );

                    StorageUtilities.SafeSaveToFile<SpriteSheet>(
                        path,
                        this.ViewModel.SpriteSheet.Model,
                        new Atom.Xna.SpriteSheet.ReaderWriter( new NullSpriteLoader() )
                    );

                    MessageBox.Show(
                        string.Format(
                            System.Globalization.CultureInfo.CurrentCulture,
                            "'{0}' has been saved!",
                            this.ViewModel.SpriteSheet.Name
                        ),
                        string.Empty,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                } );
            }

            /// <summary>
            /// Gets the full path under wich the SpriteSheet should be saved.
            /// </summary>
            /// <returns>
            /// A full filepath.
            /// </returns>
            private string GetSavePath()
            {
                string fileName = this.ViewModel.SpriteSheet.Name + Atom.Xna.SpriteSheet.ReaderWriter.Extension;
                string basePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, @"Content\Sheets\" );

                return Path.Combine( basePath, fileName );
            }
        }
    }
}
