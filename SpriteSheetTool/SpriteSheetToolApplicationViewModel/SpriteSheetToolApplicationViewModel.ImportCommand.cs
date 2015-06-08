// <copyright file="SpriteSheetToolApplicationViewModel.ImportCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetToolApplicationViewModel.ImportCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System.IO;
    using Atom.Storage;
    using Atom.Wpf;
    using Atom.Xna;
    using Microsoft.Win32;

    /// <content>
    /// Contains the ImportCommand of the SpriteSheetToolApplicationViewModel class.
    /// </content>
    public partial class SpriteSheetToolApplicationViewModel
    {
        /// <summary>
        /// Defines the ICommand that when executed allows the user to import
        /// SpriteSheets saved as XML.
        /// </summary>
        private sealed class ImportCommand : ViewModelCommand<SpriteSheetToolApplicationViewModel, SpriteSheetToolApplication>
        {
            /// <summary>
            /// Initializes a new instance of the ImportCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteSheetToolApplicationViewModel that owns the new ImportCommand.
            /// </param>
            public ImportCommand( SpriteSheetToolApplicationViewModel viewModel )
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
                var dialog = new OpenFileDialog() {
                    Filter = "Sprite Sheets XML (*.sprsh)|*.sprsh"                     
                };

                if( dialog.ShowDialog() == true )
                {
                    SafeExecute.WithMsgBox( () => {
                        var data = XmlUtilities.Deserialize<SpriteSheetData>( dialog.FileName );
                        var spriteLoader = this.ViewModel.SpriteLoader;
                        var sheet = new SpriteSheet() {
                            Name = data.Name
                        };

                        foreach( string spriteName in data.SpriteNames )
                        { 
                            ISprite sprite = null;

                            if( spriteName.Length > 0 )
                            {
                                sprite = spriteLoader.Load( TransformSpriteName( spriteName ) );
                            }

                            sheet.Add( sprite );
                        }

                        this.ViewModel.SpriteSheet = new SpriteSheetViewModel( sheet, this.ViewModel.SpriteSource );
                    });
                }
            }


            private static string TransformSpriteName( string spriteName )
            {
                spriteName = Path.ChangeExtension( spriteName, null );
                return spriteName;
            }
        }
    }
}
