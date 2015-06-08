// <copyright file="SpriteSheetViewModel.RemoveSelectedSpritesCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetViewModel.RemoveSelectedSpritesCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System.Globalization;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Contains the RemoveSelectedSpritesCommand of the SpriteSheetViewModel class.
    /// </content>
    public partial class SpriteSheetViewModel
    {
        /// <summary>
        /// Defines the ICommand that when executed asks the users
        /// whether he wants to remove the currently selected sprite.
        /// </summary>
        private sealed class RemoveSelectedSpritesCommand : ViewModelCommand<SpriteSheetViewModel, SpriteSheet>
        {
            /// <summary>
            /// Initializes a new instance of the RemoveSelectedSpritesCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteSheetViewModel that owns the new OpenCommand.
            /// </param>
            public RemoveSelectedSpritesCommand( SpriteSheetViewModel viewModel )
                : base( viewModel )
            {
                viewModel.SelectedSpriteChanged += (sender, e) => this.OnCanExecuteChanged();
            }

            /// <summary>
            /// Gets a value indicating whether this ICommand
            /// can currently be executed.
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
                return this.ViewModel.SelectedSprite != null;
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

                string question = string.Format(
                    CultureInfo.CurrentCulture,
                    "Do you really want to remove '{0}' (index {1}) from the sheet?",
                    this.ViewModel.SelectedSprite.Name,
                    this.ViewModel.SelectedSpriteIndex.ToString( CultureInfo.CurrentCulture )
                );

                if( QuestionMessageBox.Show( question ) )
                {
                    this.Model.RemoveAt( this.ViewModel.SelectedSpriteIndex );
                    this.ViewModel.sprites.RemoveAt( this.ViewModel.SelectedSpriteIndex );
                }
            }
        }
    }
}
