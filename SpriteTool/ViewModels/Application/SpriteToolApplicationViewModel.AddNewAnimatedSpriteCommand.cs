// <copyright file="SpriteToolApplicationViewModel.AddNewAnimatedSpriteCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteToolApplicationViewModel.AddNewAnimatedSpriteCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using Atom.Wpf;

    /// <content>
    /// Contains the AddNewAnimatedSpriteCommand of the SpriteToolApplicationViewModel class.
    /// </content>
    public partial class SpriteToolApplicationViewModel
    {
        /// <summary>
        /// Defines an ICommand that when executed adds a new -empty- AnimatedSprite to the SpriteDatabase.
        /// </summary>
        private sealed class AddNewAnimatedSpriteCommand : ViewModelCommand<SpriteToolApplicationViewModel, SpriteToolApplication>
        {
            /// <summary>
            /// Initializes a new instance of the AddNewAnimatedSpriteCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteToolApplicationViewModel that owns the new AddNewAnimatedSpriteCommand.
            /// </param>
            public AddNewAnimatedSpriteCommand( SpriteToolApplicationViewModel viewModel )
                : base( viewModel )
            {
                viewModel.SpriteDatabaseChanged += ( sender, e ) => this.OnCanExecuteChanged();
            }

            /// <summary>
            /// Gets a value indicating whether this ICommand can
            /// currently be executed.
            /// </summary>
            /// <param name="parameter">
            /// The paramter passed to the command.
            /// </param>
            /// <returns>
            /// true if it can be executed;
            /// otherwise false.
            /// </returns>
            public override bool CanExecute( object parameter )
            {
                return this.ViewModel.SpriteDatabase != null;
            }

            /// <summary>
            /// Executes this ICommand.
            /// </summary>
            /// <param name="parameter">
            /// The paramter passed to the command.
            /// </param>
            public override void Execute( object parameter )
            {
                if( !this.CanExecute( parameter ) )
                    return;

                this.ViewModel.SpriteDatabase.AddNewAnimatedSprite.Execute( parameter );
            }
        }
    }
}
