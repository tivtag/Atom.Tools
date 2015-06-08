// <copyright file="SpriteDatabaseViewModel.RemoveSelectedAnimatedSpriteCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteDatabaseViewModel.RemoveSelectedAnimatedSpriteCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System.Windows;
    using Atom.Tools.SpriteTool.Properties;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Contains the RemoveSelectedAnimatedSpriteCommand class.
    /// </content>
    public partial class SpriteDatabaseViewModel
    {
        /// <summary>
        /// Implements an ICommand that when executed removes the currently selected Animated Sprite.
        /// </summary>
        private sealed class RemoveSelectedAnimatedSpriteCommand : ViewModelCommand<SpriteDatabaseViewModel, SpriteDatabase>
        {
            /// <summary>
            /// Initializes a new instance of the RemoveSelectedAnimatedSpriteCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteDatabaseViewModel that owns the new RemoveSelectedAnimatedSpriteCommand.
            /// </param>
            public RemoveSelectedAnimatedSpriteCommand( SpriteDatabaseViewModel viewModel )
                : base( viewModel )
            {
                viewModel.SelectedSpriteChanged += (sender, e) => this.OnCanExecuteChanged();
            }

            /// <summary>
            /// Gets a value indicating whether this ICommand can currently
            /// be executed.
            /// </summary>
            /// <param name="parameter">
            /// The parameter passed to the command.
            /// </param>
            /// <returns>
            /// true if it can be executed;
            /// otherwise false.
            /// </returns>
            public override bool CanExecute( object parameter )
            {
                return this.ViewModel.SelectedSprite is AnimatedSpriteViewModel;
            }

            /// <summary>
            /// Executes this ICommand.
            /// </summary>
            /// <param name="parameter">
            /// The parameter passed to the command.
            /// </param>
            public override void Execute( object parameter )
            {
                if( !this.CanExecute( parameter ) )
                    return;

                var animatedSprite = (AnimatedSpriteViewModel)this.ViewModel.SelectedSprite;

                if( this.ShouldDelete( animatedSprite ) )
                {
                    this.Model.AnimatedSprites.Remove( animatedSprite.Model );
                    this.ViewModel.animatedSprites.Remove( animatedSprite );
                }
            }

            /// <summary>
            /// Gets a value indicating whether the given AnimatedSprite should
            /// really be deleted.
            /// </summary>
            /// <param name="animatedSprite">
            /// The sprite to delete.
            /// </param>
            /// <returns>
            /// true if it should be deleted;
            /// otherwise false.
            /// </returns>
            private bool ShouldDelete( AnimatedSpriteViewModel animatedSprite )
            {
                string message = string.Format(
                    System.Globalization.CultureInfo.CurrentCulture,
                    "Do you really wish to remove the selected sprite '{0}'?",
                    animatedSprite.Name ?? string.Empty
                );

                return MessageBoxResult.Yes == MessageBox.Show( 
                    message,
                    Resources.Question,
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question
                );
            }
        }
    }
}
