// <copyright file="AnimatedSpriteViewModel.MoveFrameDownCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpriteViewModel.MoveFrameDownCommand class.
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
    /// Defines the MoveFrameDownCommand of the AnimatedSpriteViewModel class.
    /// </content>
    public partial class AnimatedSpriteViewModel
    {
        /// <summary>
        /// Defines an ICommand that when executed moves
        /// the currently selected AnimatedSpriteFrame down by one index.
        /// </summary>
        private sealed class MoveFrameDownCommand : ViewModelCommand<AnimatedSpriteViewModel, AnimatedSprite>
        {
            /// <summary>
            /// Initializes a new instance of the MoveFrameDownCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The AnimatedSpriteViewModel that owns the new MoveFrameDownCommand.
            /// </param>
            public MoveFrameDownCommand( AnimatedSpriteViewModel viewModel )
                : base( viewModel )
            {
                viewModel.framesView.CurrentChanged += (sender, e) => { this.OnCanExecuteChanged(); };
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

                int nextIndex = this.ViewModel.SelectedFrameIndex + 1;
                return nextIndex < this.ViewModel.frames.Count;
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
                int nextFrameIndex = frameIndex + 1;

                this.Model.SwapFrames( frameIndex, nextFrameIndex );                
                this.ViewModel.frames.SwapItems( frameIndex, nextFrameIndex );
                this.ViewModel.framesView.MoveCurrentTo( frame );
            }
        }
    }
}
