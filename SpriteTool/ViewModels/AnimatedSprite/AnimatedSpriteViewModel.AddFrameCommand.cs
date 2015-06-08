// <copyright file="AnimatedSpriteViewModel.AddFrameCommand.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpriteViewModel.AddFrameCommand class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using Atom.Math;
    using Atom.Wpf;
    using Atom.Xna;

    /// <content>
    /// Defines the AddFrameCommand of the AnimatedSpriteViewModel class.
    /// </content>
    public partial class AnimatedSpriteViewModel 
    {
        /// <summary>
        /// Implements a command that adds a new AnimatedSpriteFrame to the AnimatedSprite.
        /// </summary>
        private sealed class AddFrameCommand : ViewModelCommand<AnimatedSpriteViewModel, AnimatedSprite>
        {
            /// <summary>
            /// Initializes a new instance of the AddFrameCommand class.
            /// </summary>
            /// <param name="viewModel">
            /// The AnimatedSpriteViewModel that owns the new AddFrameCommand.
            /// </param>
            /// <param name="spriteLoader">
            /// Provides a mechanism that allows loading of <see cref="Sprite"/> assets.
            /// </param>
            public AddFrameCommand( AnimatedSpriteViewModel viewModel, INormalSpriteLoader spriteLoader )
                : base( viewModel )
            {
                this.spriteLoader = spriteLoader;
            }

            /// <summary>
            /// Executes this ICommand.
            /// </summary>
            /// <param name="parameter">
            /// The parameters that are passed to the command.
            /// </param>
            public override void Execute( object parameter )
            {
                var frame = this.Model.AddFrame(
                    this.GetSprite(), 
                    Vector2.Zero,
                    100.0f 
                );

                this.ViewModel.frames.Add( frame );
            }

            /// <summary>
            /// Gets the default Sprite of the new AnimatedSpriteFrame.
            /// </summary>
            /// <returns>
            /// The Sprite that has been choosen;
            /// might be null.
            /// </returns>
            private Sprite GetSprite()
            {
                Sprite lastSprite = this.GetLastSprite();
                string nextSpriteName = this.GetNextSpriteName( lastSprite );

                if( nextSpriteName == null )
                    return lastSprite;

                try
                {
                    return this.spriteLoader.LoadSprite( nextSpriteName );
                }
                catch
                {
                    return lastSprite;
                }
            }

            /// <summary>
            /// Gets the Sprite of the last frame that has a non-null Sprite.
            /// </summary>
            /// <returns>
            /// The last Sprite;
            /// or null if there are no frames or none of them have a Sprite.
            /// </returns>
            private Sprite GetLastSprite()
            {
                for( int index = this.Model.FrameCount - 1; index >= 0; --index )
                {
                    AnimatedSpriteFrame frame = this.Model[index];
                    Sprite sprite = frame.Sprite;

                    if( sprite != null )
                    {
                        return sprite;
                    }                    
                }

                return null;
            }

            /// <summary>
            /// Gets the name of the next sprite.
            /// </summary>
            /// <param name="lastSprite">
            /// The last sprite.
            /// </param>
            /// <returns>
            /// The name that uniquely identifies the next name.
            /// </returns>
            private string GetNextSpriteName( Sprite lastSprite )
            {
                if( lastSprite != null )
                {
                    return StringUtilities.IncrementTrailingInteger( lastSprite.Name );
                }
                else
                {
                    if( !string.IsNullOrEmpty( this.Model.Name ) )
                    {
                        return this.Model.Name + "_1";
                    }

                    return null;
                }
            }   

            /// <summary>
            /// Provides a mechanism that allows loading of <see cref="Sprite"/> assets.
            /// </summary>
            private readonly INormalSpriteLoader spriteLoader;
        }
    }
}
