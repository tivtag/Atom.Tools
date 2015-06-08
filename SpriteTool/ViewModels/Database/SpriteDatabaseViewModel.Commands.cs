// <copyright file="SpriteDatabaseViewModel.Commands.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the public visible commands of the Atom.Tools.SpriteTool.SpriteDatabaseViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System.Windows.Input;
    using Atom.Xna;

    /// <content>
    /// Contains the ICommand properties and their related initialization logic.
    /// </content>
    public partial class SpriteDatabaseViewModel
    {
        /// <summary>
        /// Gets the ICommand that when executed adds a new AnimatedSprite to the SpriteDatabase.
        /// </summary>
        public ICommand AddNewAnimatedSprite
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed removes the currently selected AnimatedSprite
        /// from the SpriteDatabase.
        /// </summary>
        public ICommand RemoveSelectedAnimatedSprite
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and initializes the ICommands this SpriteDatabaseViewModel exposes
        /// </summary>
        private void InitializeCommands()
        {
            this.AddNewAnimatedSprite = new AddNewAnimatedSpriteCommand( this );
            this.RemoveSelectedAnimatedSprite = new RemoveSelectedAnimatedSpriteCommand( this );
        }
    }
}
