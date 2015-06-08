// <copyright file="AnimatedSpriteViewModel.PlayCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpriteViewModel.PlayCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using Atom.Xna;
    using Atom.Wpf;

    /// <content>
    /// Defines the PlayCommand of the AnimatedSpriteViewModel class.
    /// </content>
    public partial class AnimatedSpriteViewModel
    {
        /// <summary>
        /// Defines an ICommand that when executed starts animating
        /// of the SpriteAnimation.
        /// </summary>
        private sealed class PlayCommand : ViewModelCommand<AnimatedSpriteViewModel, AnimatedSprite>
        {
            /// <summary>
            /// Initializes a new instance of the SetFrameSpriteCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The AnimatedSpriteViewModel that owns the new SetFrameSpriteCommand.
            /// </param>
            public PlayCommand( AnimatedSpriteViewModel viewModel )
                : base( viewModel )
            {
                viewModel.PropertyChanged += (sender, e) => {
                    if( e.PropertyName == "IsAnimationPlaying" )
                    {
                        this.OnCanExecuteChanged();
                    }
                };
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
                return !this.ViewModel.IsAnimationPlaying;
            }

            /// <summary>
            /// Executes this SetFrameSpriteCommand
            /// </summary>
            /// <param name="parameter">
            /// The parameter that has been passed to the ICommand.
            /// </param>
            public override void Execute( object parameter )
            {
                this.ViewModel.IsAnimationPlaying = true;
            }
        }
    }
}
