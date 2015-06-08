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
        private sealed class AddDummySpriteCommand : ViewModelCommand<SpriteSheetViewModel, SpriteSheet>
        {
            /// <summary>
            /// Initializes a new instance of the AddDummySpriteCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteSheetViewModel that owns the new OpenCommand.
            /// </param>
            public AddDummySpriteCommand( SpriteSheetViewModel viewModel )
                : base( viewModel )
            {
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
                
                this.Model.Add( null );
                this.ViewModel.sprites.Add( null );
            }
        }
    }
}
