// <copyright file="AnimatedSpriteViewModel.SetFrameSpriteCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpriteViewModel.SetFrameSpriteCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using Atom.Wpf;
    using Atom.Wpf.Dialogs;
    using Atom.Xna;

    /// <content>
    /// Defines the SetFrameSpriteCommand of the AnimatedSpriteViewModel class.
    /// </content>
    public partial class AnimatedSpriteViewModel
    {
        /// <summary>
        /// Defines an ICommand that when executed allows
        /// the user to choose a sprite
        /// </summary>
        private sealed class SetFrameSpriteCommand : ViewModelCommand<AnimatedSpriteViewModel, AnimatedSprite>
        {
            /// <summary>
            /// Initializes a new instance of the SetFrameSpriteCommand class.
            /// </summary>
            /// <param name="spriteSource">
            /// Provides access to all <see cref="Sprite"/>s the AnimatedSprite
            /// may possibly contain.
            /// </param>
            /// <param name="viewModel">
            /// The AnimatedSpriteViewModel that owns the new SetFrameSpriteCommand.
            /// </param>
            public SetFrameSpriteCommand( INormalSpriteSource spriteSource, AnimatedSpriteViewModel viewModel )
                : base( viewModel )
            {
                if( spriteSource == null )
                    throw new ArgumentNullException( "spriteSource" );

                viewModel.framesView.CurrentChanged += ( sender, e ) => { this.OnCanExecuteChanged(); };
                this.spriteSource = spriteSource;
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

                var dialog = new ItemSelectionDialog<Sprite>( this.spriteSource.Sprites );

                if( dialog.ShowDialog() == true )
                {
                    var frame = this.ViewModel.SelectedFrame;
                    frame.Sprite = dialog.SelectedItem;
                }
            }

            /// <summary>
            /// Provides access to all <see cref="Sprite"/>s the AnimatedSprite
            /// may possibly contain.
            /// </summary>
            private readonly INormalSpriteSource spriteSource;
        }
    }
}
