// <copyright file="SpriteSheetToolApplicationViewModel.Commands.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the ICommand properties and initialization logic for the
//     Atom.Tools.SpriteSheetTool.SpriteSheetToolApplicationViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System.Windows.Input;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Contains the ICommand properties and initialization logic for the 
    /// SpriteSheetToolApplicationViewModel class.
    /// </content>
    public partial class SpriteSheetToolApplicationViewModel
    {
        /// <summary>
        /// Gets the ICommand that when executed creates
        /// a new -empty- SpriteSheet.
        /// </summary>
        public ICommand New
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed allows the user
        /// to open an existing SpriteSheet.
        /// </summary>
        public ICommand Open
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed saves
        /// the current SpriteSheet.
        /// </summary>
        public ICommand Save
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed allows the user
        /// to open an existing SpriteSheet.
        /// </summary>
        public ICommand Import
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed zooms in the view by a preset amount.
        /// </summary>
        public ICommand ZoomView
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed resets the view to default values.
        /// </summary>
        public ICommand ResetView
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the commands of this SpriteSheetToolApplicationViewModel.
        /// </summary>
        private void InitializeCommands()
        {
            this.New = new NewCommand( this );
            this.Save = new SaveCommand( this );
            this.Open = new OpenCommand( this );
            this.Import = new ImportCommand( this );

            this.ZoomView = new LambdaCommand( _ => {
                this.XnaApplication.ZoomView( 1.0f );
            } );

            this.ResetView = new LambdaCommand( _ => {
                this.XnaApplication.ResetView();
            } );
        }
    }
}
