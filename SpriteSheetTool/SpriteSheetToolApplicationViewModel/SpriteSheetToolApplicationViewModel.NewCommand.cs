// <copyright file="SpriteSheetToolApplicationViewModel.NewCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetToolApplicationViewModel.NewCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using Atom.Storage;
    using Atom.Wpf;
    using Atom.Xna;
    using Microsoft.Win32;
    using System.IO;
    using System;

    /// <content>
    /// Contains the NewCommand of the SpriteSheetToolApplicationViewModel class.
    /// </content>
    public partial class SpriteSheetToolApplicationViewModel
    {
        /// <summary>
        /// Defines the ICommand that when executed creates
        /// a new -empty- SpriteSheet.
        /// </summary>
        private sealed class NewCommand : ViewModelCommand<SpriteSheetToolApplicationViewModel, SpriteSheetToolApplication>
        {
            /// <summary>
            /// Initializes a new instance of the NewCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The SpriteSheetToolApplicationViewModel that owns the new NewCommand.
            /// </param>
            public NewCommand( SpriteSheetToolApplicationViewModel viewModel )
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
                var sheet = new SpriteSheet();
                sheet.Add( null );

                this.ViewModel.SpriteSheet = new SpriteSheetViewModel( sheet, this.ViewModel.SpriteSource );
            }
        }
    }
}
