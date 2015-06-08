// <copyright file="SpriteSheetViewModel.MoveSelectedSpriteUpCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetViewModel.MoveSelectedSpriteUpCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using Atom.Collections;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Contains the MoveSelectedSpriteUpCommand of the SpriteSheetViewModel class.
    /// </content>
    public partial class SpriteSheetViewModel
    {
        /// <summary>
        /// Defines the ICommand that when executed moves the currently
        /// selected sprite up by one index.
        /// </summary>
        private sealed class MoveSelectedSpriteUpCommand : ViewModelCommand<SpriteSheetViewModel, SpriteSheet>
        {
            /// <summary>
            /// Initializes a new instance of the MoveSelectedSpriteUpCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteSheetViewModel that owns the new OpenCommand.
            /// </param>
            public MoveSelectedSpriteUpCommand( SpriteSheetViewModel viewModel )
                : base( viewModel )
            {
                viewModel.SelectedSpriteChanged += ( sender, e ) => this.OnCanExecuteChanged();
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
                return this.ViewModel.SelectedSprite != null && this.ViewModel.SelectedSpriteIndex > 1;
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

                int selectedIndex = this.ViewModel.SelectedSpriteIndex;
                int otherIndex = selectedIndex - 1;

                this.Model.SwapItems( selectedIndex, otherIndex );
                this.ViewModel.sprites.SwapItems( selectedIndex, otherIndex );
                this.ViewModel.spritesView.Refresh();
                this.ViewModel.spritesView.MoveCurrentToPosition( otherIndex );
            }
        }
    }
}
