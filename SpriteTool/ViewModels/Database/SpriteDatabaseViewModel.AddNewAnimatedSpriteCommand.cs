// <copyright file="SpriteDatabaseViewModel.AddNewAnimatedSpriteCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteDatabaseViewModel.AddNewAnimatedSpriteCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.Linq;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Contains the AddAnimatedSpriteCommand class.
    /// </content>
    public partial class SpriteDatabaseViewModel
    {
        /// <summary>
        /// Implements an ICommand that when executed adds a new Animated Sprite to the SpriteDatabase.
        /// </summary>
        private sealed class AddNewAnimatedSpriteCommand : ViewModelCommand<SpriteDatabaseViewModel, SpriteDatabase>
        {
            /// <summary>
            /// Initializes a new instance of the AddNewAnimatedSpriteCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteDatabaseViewModel that owns the new AddNewAnimatedSpriteCommand.
            /// </param>
            public AddNewAnimatedSpriteCommand( SpriteDatabaseViewModel viewModel )
                : base( viewModel )
            {
            }

            /// <summary>
            /// Executes this ICommand.
            /// </summary>
            /// <param name="parameter">
            /// The parameter passed to the command.
            /// </param>
            public override void Execute( object parameter )
            {
                var animatedSprite = AnimatedSprite.CreateManual( this.GetName(), 0 );

                var viewModel = this.ViewModel.AddAnimatedSprite( animatedSprite );
                this.ViewModel.animatedSpritesView.MoveCurrentToLast();
            }
            
            /// <summary>
            /// Gets an unique name the new AnimatedSprite has by default. 
            /// </summary>
            /// <returns>
            /// A unique name.
            /// </returns>
            private string GetName()
            {
                int index = 0;
                string name = "NewAnimatedSprite";

                while( this.Model.AnimatedSprites.Any( sprite => sprite.Name.Equals( name, StringComparison.Ordinal ) ) )
                {
                    name = "NewAnimatedSprite_" + (++index).ToString(); 
                }

                return name;
            }
        }
    }
}
