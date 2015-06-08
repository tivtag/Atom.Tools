// <copyright file="SpriteSheetViewModel.Commands.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the ICommand properties and initialization logic for the
//     Atom.Tools.SpriteSheetTool.SpriteSheetViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System.Windows.Input;
    using Atom.Xna;

    /// <content>
    /// Contains the ICommand properties and initialization logic for the 
    /// SpriteSheetViewModel class.
    /// </content>
    public partial class SpriteSheetViewModel
    {
        /// <summary>
        /// Gets the ICommand that when executed allows
        /// the user to add ISprites to the SpriteSheet.
        /// </summary>
        public ICommand AddSprites
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed removes
        /// the currently selected ISprite.
        /// </summary>
        public ICommand RemoveSelectedSprite
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed moves
        /// the currently selected ISprite down by one index in the SpriteSheet.
        /// </summary>
        public ICommand MoveSelectedSpriteDown
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed moves
        /// the currently selected ISprite up by one index in the SpriteSheet.
        /// </summary>
        public ICommand MoveSelectedSpriteUp
        {
            get;
            private set;
        }

        public ICommand AddDummySprite
        {
            get;
            private set;
        }        

        /// <summary>
        /// Initializes the commands of this SpriteSheetViewModel.
        /// </summary>
        /// <param name="spriteSource">
        /// Provides access to the ISprites that may be added to the SpriteSheet.
        /// </param>
        private void InitializeCommands( ISpriteSource spriteSource )
        {
            this.AddSprites = new AddSpritesCommand( spriteSource, this );
            this.AddDummySprite = new AddDummySpriteCommand( this );
            this.RemoveSelectedSprite = new RemoveSelectedSpritesCommand( this );
            this.MoveSelectedSpriteDown = new MoveSelectedSpriteDownCommand( this );
            this.MoveSelectedSpriteUp = new MoveSelectedSpriteUpCommand( this );
        }
    }
}
