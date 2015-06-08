// <copyright file="AnimatedSpriteViewModel.RemoveSelectedFrameCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpriteViewModel.RemoveSelectedFrameCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using Atom.Xna;
    using Atom.Wpf;
    using System.Windows;

    /// <content>
    /// Defines the RemoveSelectedFrameCommand of the AnimatedSpriteViewModel class.
    /// </content>
    public partial class AnimatedSpriteViewModel
    {
        /// <summary>
        /// Defines an ICommand that when executed allows
        /// the user to choose a sprite
        /// </summary>
        private sealed class RemoveSelectedFrameCommand : ViewModelCommand<AnimatedSpriteViewModel, AnimatedSprite>
        {
            /// <summary>
            /// Initializes a new instance of the RemoveSelectedFrameCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The AnimatedSpriteViewModel that owns the new RemoveSelectedFrameCommand.
            /// </param>
            public RemoveSelectedFrameCommand( AnimatedSpriteViewModel viewModel )
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
                return this.ViewModel.SelectedFrame != null;
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
                
                if( this.ShouldReallyRemove() )
                {
                    var frame = this.ViewModel.SelectedFrame;

                    this.Model.RemoveFrame( frame );
                    this.ViewModel.frames.Remove( frame );
                }
            }

            /// <summary>
            /// Asks the user whether he really wants to remove
            /// the currently selected frame.
            /// </summary>
            /// <returns>
            /// true if he wants to remove it;
            /// otherwise false.
            /// </returns>
            private bool ShouldReallyRemove()
            {
                var frame = this.ViewModel.SelectedFrame;
                int index = this.ViewModel.SelectedFrameIndex;

                string question = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    "Do you really wish to remove the {0}th frame{1}?",
                    (index + 1).ToString( System.Globalization.CultureInfo.CurrentCulture ),
                    frame.Sprite != null ? " " + frame.Sprite.Name : string.Empty
                );

                return QuestionMessageBox.Show( question );
            }
        }
    }
}
