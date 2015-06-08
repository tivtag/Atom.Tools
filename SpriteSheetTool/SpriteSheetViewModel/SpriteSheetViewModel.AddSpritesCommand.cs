// <copyright file="SpriteSheetViewModel.AddSpritesCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetViewModel.AddSpritesCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System.Linq;
    using Atom.Wpf;
    using Atom.Wpf.Dialogs;
    using Atom.Xna;

    /// <content>
    /// Contains the AddSpritesCommand of the SpriteSheetViewModel class.
    /// </content>
    public partial class SpriteSheetViewModel
    {
        /// <summary>
        /// Defines the ICommand that when executed allows
        /// the user to add ISprites to the SpriteSheet.
        /// </summary>
        private sealed class AddSpritesCommand : ViewModelCommand<SpriteSheetViewModel, SpriteSheet>
        {
            /// <summary>
            /// Initializes a new instance of the AddSpritesCommand class.
            /// </summary>
            /// <param name="spriteSource">
            /// Provides access to the ISprites that the user is allowed to select.
            /// </param>
            /// <param name="viewModel">
            /// The SpriteSheetViewModel that owns the new OpenCommand.
            /// </param>
            public AddSpritesCommand( ISpriteSource spriteSource, SpriteSheetViewModel viewModel )
                : base( viewModel )
            {
                this.assets = spriteSource.Sprites.Concat<IAsset>( spriteSource.AnimatedSprites );
                
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
                
                var dialog = new ItemSelectionDialog<IAsset>( this.assets ) {
                    AllowMultipleSelection = true
                };

                if( dialog.ShowDialog() == true )
                {
                    foreach( IAsset asset in dialog.SelectedItems )
                    {
                        ISprite sprite = CreateSprite( asset );

                        this.Model.Add( sprite );
                        this.ViewModel.sprites.Add( sprite );
                    }
                }
            }

            /// <summary>
            /// Creates an ISprite from the given IAsset.
            /// </summary>
            /// <param name="asset">
            /// The input asset.
            /// </param>
            /// <returns>
            /// The drawable output ISprite.
            /// </returns>
            private ISprite CreateSprite( IAsset asset )
            {
                var sprite = asset as Sprite;

                if( sprite != null )
                {
                    return sprite;
                }
                else
                {
                    var animatedSprite = (AnimatedSprite)asset;
                    return animatedSprite.CreateInstance();
                }
            }

            private readonly System.Collections.Generic.IEnumerable<IAsset> assets;
        }
    }
}
