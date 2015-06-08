// <copyright file="AnimatedSpriteViewModel.MoveFrameUpCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpriteViewModel.MoveFrameUpCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using Atom.Collections;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Defines the MoveFrameUpCommand of the AnimatedSpriteViewModel class.
    /// </content>
    public partial class AnimatedSpriteViewModel
    {
        /// <summary>
        /// Defines an ICommand that when executed moves
        /// the currently selected AnimatedSpriteFrame down by one index.
        /// </summary>
        private sealed class MoveFrameUpCommand : ViewModelCommand<AnimatedSpriteViewModel, AnimatedSprite>
        {
            /// <summary>
            /// Initializes a new instance of the MoveFrameUpCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The AnimatedSpriteViewModel that owns the new MoveFrameUpCommand.
            /// </param>
            public MoveFrameUpCommand( AnimatedSpriteViewModel viewModel )
                : base( viewModel )
            {
                viewModel.framesView.CurrentChanged += ( sender, e ) => { this.OnCanExecuteChanged(); };
            }

            /// <summary>
            /// Gets a value indicating whether this SetFrameSpriteCommand can
            /// currently be executed.
            /// </summary>
            /// <param name="parameter">
            /// The parameter that has been passed to the ICommand.
            /// </param>
            /// <returns>
            /// true if this ICommand can be executed;
            /// otherwise false.
            /// </returns>
            public override bool CanExecute( object parameter )
            {
                if( this.ViewModel.SelectedFrame == null )
                    return false;

                int previousIndex = this.ViewModel.SelectedFrameIndex - 1;
                return previousIndex >= 0;
            }

            /// <summary>
            /// Executes this SetFrameSpriteCommand
            /// </summary>
            /// <param name="parameter">
            /// The parameter that has been passed to the ICommand.
            /// </param>
            public override void Execute( object parameter )
            {
                if( !this.CanExecute( parameter ) )
                    return;

                AnimatedSpriteFrame frame = this.ViewModel.SelectedFrame;
                int frameIndex = this.ViewModel.SelectedFrameIndex;
                int previousFrameIndex = frameIndex - 1;

                this.Model.SwapFrames( frameIndex, previousFrameIndex );
                this.ViewModel.frames.SwapItems( frameIndex, previousFrameIndex );
                this.ViewModel.framesView.MoveCurrentTo( frame );
            }
        }
    }
}
