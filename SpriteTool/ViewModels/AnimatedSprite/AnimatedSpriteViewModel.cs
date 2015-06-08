// <copyright file="AnimatedSpriteViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpriteViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using Atom.Wpf;
    using Atom.Xna;

    /// <summary>
    /// Represents the ViewModel that wraps around the <see cref="AnimatedSprite"/> class.
    /// </summary>
    public sealed partial class AnimatedSpriteViewModel : ViewModel<AnimatedSprite>, IViewModel<ISprite>
    {
        /// <summary>
        /// Gets or sets the named of the AnimatedSprite.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Model.Name;
            }

            set
            {
                if( value == this.Name )
                    return;

                this.Model.Name = value;
                this.OnPropertyChanged( "Name" );
            }
        }

        /// <summary>
        /// Gets the <see cref="SpriteAnimation"/> that is used to visualize this AnimatedSpriteViewModel
        /// </summary>
        public SpriteAnimation SpriteAnimation
        {
            get
            {
                return this.animation;
            }
        }

        /// <summary>
        /// Gets the SpriteAnimation that is used to visualize this AnimatedSpriteViewModel
        /// </summary>
        ISprite IViewModel<ISprite>.Model
        {
            get 
            {
                return this.SpriteAnimation;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the animation is currently animating.
        /// </summary>
        public bool IsAnimationPlaying
        {
            get
            {
                return this.animation.IsAnimatingEnabled;
            }

            private set
            {
                if( value == this.IsAnimationPlaying )
                    return;

                this.animation.IsAnimatingEnabled = value;
                this.OnPropertyChanged( "IsAnimationPlaying" );
            }
        }

        /// <summary>
        /// Gets or sets the speed the animation is animated at.
        /// </summary>
        public float AnimationSpeed
        {
            get
            {
                return this.Model.DefaultAnimationSpeed;
            }

            set
            {
                if( value == this.AnimationSpeed )
                    return;

                this.animation.AnimationSpeed = value;
                this.Model.DefaultAnimationSpeed = value;

                this.OnPropertyChanged( "AnimationSpeed" );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is looping.
        /// </summary>
        public bool IsLooping
        {
            get
            {
                return this.animation.IsLooping;
            }

            set
            {
                if( value == this.IsLooping )
                    return;

                this.Model.IsLoopingByDefault = value;
                this.animation.IsLooping = value;
                this.OnPropertyChanged( "IsLooping" );
            }
        }

        /// <summary>
        /// Gets the zero-based index of the currently selected Frame.
        /// </summary>
        public int SelectedFrameIndex
        {
            get
            {
                return this.framesView.CurrentPosition;
            }
        }

        /// <summary>
        /// Gets the currently selected AnimatedSpriteFrame.
        /// </summary>
        public AnimatedSpriteFrame SelectedFrame
        {
            get
            {
                return this.framesView.CurrentItem as AnimatedSpriteFrame;
            }
        }

        /// <summary>
        /// Gets the bind-able collection of AnimatedSpriteFrames.
        /// </summary>
        public CollectionView Frames
        {
            get
            {
                return this.framesView;
            }
        }

        /// <summary>
        /// Initializes a new instance of the AnimatedSpriteViewModel class.
        /// </summary>
        /// <param name="model">
        /// The AnimatedSprite the new AnimatedSpriteViewModel wraps around.
        /// </param>
        /// <param name="spriteSource">
        /// Provides access to all <see cref="Sprite"/>s the AnimatedSprite
        /// may possibly contain.
        /// </param>
        /// <param name="spriteLoader">
        /// Provides a mechanism that allows the loading of Sprite assets.
        /// </param>
        public AnimatedSpriteViewModel( AnimatedSprite model, INormalSpriteSource spriteSource, INormalSpriteLoader spriteLoader )
            : base( model )
        {
            for( int index = 0; index < model.FrameCount; ++index )
            {
                this.frames.Add( model[index] );
            }

            this.framesView = new ListCollectionView( this.frames );
            this.framesView.CurrentChanged += this.OnCurrentFrameChanged;
                        
            this.animation = new SpriteAnimation( model );
            this.InitializeCommands( spriteSource, spriteLoader );
        }

        /// <summary>
        /// Called when the currently selected frame has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The current EventArgs.
        /// </param>
        private void OnCurrentFrameChanged( object sender, EventArgs e )
        {
            if( !this.IsAnimationPlaying )
            {
                int frameIndex = this.SelectedFrameIndex;

                if( frameIndex != -1 )
                {
                    this.animation.FrameIndex = frameIndex;
                }
            }

            this.OnPropertyChanged( "SelectedFrame" );
            this.OnPropertyChanged( "SelectedFrameIndex" );
        }

        /// <summary>
        /// The CollectionView that wraps around the list of AnimatedSpriteFrames.
        /// </summary>
        private readonly ListCollectionView framesView;

        /// <summary>
        /// The observable list of AnimatedSprites.
        /// </summary>
        private readonly ObservableCollection<AnimatedSpriteFrame> frames = new ObservableCollection<AnimatedSpriteFrame>();

        /// <summary>
        /// The SpriteAnimation that is used to visualize an instance of the AnimatedSprite.
        /// </summary>
        private readonly SpriteAnimation animation;
    }
}
