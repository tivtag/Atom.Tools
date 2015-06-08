// <copyright file="XnaApplication.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.XnaApplication class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using Atom.Math;
    using Atom.Xna;
    using Atom.Xna.Batches;
    using Atom.Xna.Wpf;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents the Xna Game that is running in the background
    /// of the WPF SpriteTool application.
    /// </summary>
    public sealed class XnaApplication : XnaWpfGame
    {
        /// <summary>
        /// Gets the ITexture2DLoader that can be used to load the Texture2D
        /// assets used by sprites.
        /// </summary>
        public ITexture2DLoader TextureLoader
        {
            get
            {
                return this.textureLoader;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying texture
        /// of the current sprite should be shown; or the sprite in its own.
        /// </summary>
        public bool DesignMode
        {
            get;
            set;
        }

        /// <summary>
        /// The size of the view area.
        /// </summary>
        private static readonly Point2 ViewSize = new Point2( 636, 703 );

        /// <summary>
        /// Initializes a new instance of the XnaApplication class.
        /// </summary>
        /// <param name="controlHandle">
        /// The handle of the control in which Xna is drawing.
        /// </param>
        public XnaApplication( IntPtr controlHandle )
            : base( ViewSize, controlHandle )
        {
            this.Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.textureLoader = new UncachedFromFileTexture2DLoader( this.Graphics );
        }

        /// <summary>
        /// Loads the content used by this XnaApplication.
        /// </summary>
        protected override void LoadContent()
        {
            this.drawContext = new SpriteDrawContext( new ComposedSpriteBatch( this.GraphicsDevice ), this.GraphicsDevice );
        }

        /// <summary>
        /// Initializes this XnaApplication.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        }

        /// <summary>
        /// Shows the given ISprite in the xna draw area.
        /// </summary>
        /// <param name="sprite">
        /// The current 
        /// </param>
        public void ShowSprite( ISprite sprite )
        {
            if( sprite == this.sprite )
                return;

            this.sprite = sprite;
        }

        /// <summary>
        /// Moves the view.
        /// </summary>
        /// <param name="offset"></param>
        public void TranslateView( Vector2 offset )
        {
            if( sprite == null )
                return;

            if( this.DesignMode )
                this.viewTransformDesign.Translation += new Vector3( (int)offset.X/2, (int)offset.Y/2, 0.0f );
            else
                this.viewTransform.Translation += new Vector3( (int)offset.X/2, (int)offset.Y/2, 0.0f );
        }

        /// <summary>
        /// Resets the view transformation.
        /// </summary>
        public void ResetView()
        {
            if( this.DesignMode )
                this.viewTransformDesign = Matrix4.Identity;
            else
                this.viewTransform = Matrix4.Identity;
        }

        /// <summary>
        /// Zooms the view in or out.
        /// </summary>
        /// <param name="factor">
        /// The zoom factor to apply.
        /// </param>
        public void ZoomView( float factor )
        {
            if( sprite == null )
                return;

            if( this.DesignMode )
            {
                this.viewTransformDesign.M11 += factor;
                this.viewTransformDesign.M22 += factor;
            }
            else
            {
                this.viewTransform.M11 += factor;
                this.viewTransform.M22 += factor;
            }
        }

        /// <summary>
        /// Draws the next frame.
        /// </summary>
        /// <param name="gameTime">
        /// The current GameTime.
        /// </param>
        protected override void Draw( Xna.GameTime gameTime )
        {
            this.GraphicsDevice.Clear( Xna.Color.Black );
            this.drawContext.GameTime = gameTime;

            this.DrawSprite();
        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawSprite()
        {
            if( this.sprite == null || this.sprite.Texture == null )
                return;
            
            this.drawContext.Begin( BlendState.Opaque, SamplerState.PointClamp, SpriteSortMode.Immediate, this.GetViewTransform() );
            {
                var batch = this.drawContext.Batch;

                if( this.DesignMode )
                {
                    batch.Draw(
                        this.sprite.Texture, 
                        Vector2.Zero, 
                        new Xna.Color( 255, 255, 255, 111 )
                    );

                    var animation = sprite as SpriteAnimation;

                    if( animation != null )
                    {
                        if( animation.Frame != null )
                        {
                            var frameSprite = animation.Frame.Sprite;

                            if( frameSprite != null )
                            {
                                frameSprite.Draw(
                                    new Vector2( frameSprite.Source.X, frameSprite.Source.Y ),
                                    batch
                                );
                            }
                            else
                            {
                                Console.WriteLine( "no sprite" );
                            }
                        }
                        else
                        {
                            Console.WriteLine( "no frame" );
                        }
                    }
                    else
                    {
                        this.sprite.Draw( 
                            new Vector2( this.sprite.Source.X, this.sprite.Source.Y ),
                            batch
                        );
                    }
                }
                else
                {
                    this.sprite.Draw( Vector2.Zero, batch );
                }
            }

            this.drawContext.End();
        }

        /// <summary>
        /// Updates the application.
        /// </summary>
        /// <param name="gameTime">
        /// The current GameTime.
        /// </param>
        protected override void Update( Xna.GameTime gameTime )
        {
            this.updateContext.GameTime = gameTime;

            var updateable = this.sprite as IUpdateable;

            if( updateable != null )
            {
                updateable.Update( this.updateContext );
            }
        }

        /// <summary>
        /// Gets the current overall view transformation matrix.
        /// </summary>
        /// <returns></returns>
        private Matrix4 GetViewTransform()
        {
            return this.DesignMode ? this.viewTransformDesign : this.viewTransform;
        }

        /// <summary>
        /// The ISprite that is currently drawn.
        /// </summary>
        private ISprite sprite;

        /// <summary>
        /// The transform that is normally applied.
        /// </summary>
        private Matrix4 viewTransform = Matrix4.CreateTranslation( ViewSize.X / 2.0f, ViewSize.Y / 2.0f, 0.0f );

        /// <summary>
        /// The transform that is applied when in Design Mode.
        /// </summary>
        private Matrix4 viewTransformDesign = Matrix4.Identity;

        /// <summary> 
        /// The context under which each drawing operation occurs.
        /// </summary>
        private SpriteDrawContext drawContext;

        /// <summary>
        /// The context under which each update operation occurs.
        /// </summary>
        private readonly IXnaUpdateContext updateContext = new XnaUpdateContext();

        /// <summary>
        /// The ITexture2DLoader that can be used to load the Texture2D
        /// assets used by sprites.
        /// </summary>
        private readonly ITexture2DLoader textureLoader;
    }
}
