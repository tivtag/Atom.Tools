// <copyright file="AnimatedSpriteViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the initialization logic for the ICommands the
//     Atom.Tools.SpriteTool.AnimatedSpriteViewModel class exposes.
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
    public partial class AnimatedSpriteViewModel
    {
        /// <summary>
        /// Gets the ICommand that when executed adds a new AnimatedSpriteFrame to the AnimatedSprite.
        /// </summary>
        public ICommand AddFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed removes
        /// the currently selected AnimatedSpriteFrame from the AnimatedSprite.
        /// </summary>
        public ICommand RemoveSelectedFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed allows
        /// the user to set the sprite of the currently selected Sprite.
        /// </summary>
        public ICommand SetSelectedFrameSprite
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed moves
        /// the currently selected AnimatedSpriteFrame down by one index.
        /// </summary>
        public ICommand MoveSelectedFrameDown
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed moves
        /// the currently selected AnimatedSpriteFrame up by one index.
        /// </summary>
        public ICommand MoveSelectedFrameUp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed starts animating of the SpriteAnimation.
        /// </summary>
        public ICommand Play
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the ICommand that when executed stops animating of the SpriteAnimation.
        /// </summary>
        public ICommand Stop
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed pauses animating of the SpriteAnimation.
        /// </summary>
        public ICommand Pause
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and initializes the ICommands this AnimatedSpriteViewModel exposes
        /// </summary>
        /// <param name="spriteSource">
        /// Provides access to all <see cref="Sprite"/>s the AnimatedSprite
        /// may possibly contain.
        /// </param>
        /// <param name="spriteLoader">
        /// Provides a mechanism that allows loading of <see cref="Sprite"/> assets.
        /// </param>
        private void InitializeCommands( INormalSpriteSource spriteSource, INormalSpriteLoader spriteLoader )
        {
            this.AddFrame = new AddFrameCommand( this, spriteLoader );
            this.RemoveSelectedFrame = new RemoveSelectedFrameCommand( this );

            this.SetSelectedFrameSprite = new SetFrameSpriteCommand( spriteSource, this );
            this.MoveSelectedFrameDown = new MoveFrameDownCommand( this );
            this.MoveSelectedFrameUp = new MoveFrameUpCommand( this );

            this.Play = new PlayCommand( this );
            this.Stop = new StopCommand( this );
            this.Pause = new PauseCommand( this );
        }
    }
}
