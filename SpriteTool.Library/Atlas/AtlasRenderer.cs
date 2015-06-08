// <copyright file="AtlasRenderer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.AtlasRenderer class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a mechanism that renders a <see cref="TextureAtlas"/> into a <see cref="Texture2D"/>.
    /// This class can't be inherited.
    /// </summary>
    public sealed class AtlasRenderer
    {
        /// <summary>
        /// Initializes a new instance of the AtlasRenderer class.
        /// </summary>
        /// <param name="graphicsDeviceService">
        /// Provides a mechanism that allows to receive the Xna GraphicsDevice.
        /// </param>
        public AtlasRenderer( IGraphicsDeviceService graphicsDeviceService )
        {
            Contract.Requires<ArgumentNullException>( graphicsDeviceService != null );

            this.graphicsDeviceService = graphicsDeviceService;
        }

        /// <summary>
        /// Renders the given TextureAtlas to a single Texture2D.
        /// </summary>
        /// <param name="atlas">
        /// The TextureAtlas that should be rendered.
        /// </param>
        /// <param name="configuration">
        /// The configuration that was used to build the TextureAtlas.
        /// </param>
        /// <returns>
        /// The texture into which the TextureAtlas was rendered.
        /// </returns>
        public Texture2D Render( TextureAtlas atlas, AtlasConfiguration configuration )
        {
            Contract.Requires<ArgumentNullException>( atlas != null );

            this.atlas = atlas;
            this.graphicsDevice = this.graphicsDeviceService.GraphicsDevice;

            RenderTarget2D renderTarget = this.CreateRenderTarget();
            this.graphicsDevice.SetRenderTarget( renderTarget );
            this.graphicsDevice.Clear( new Color(255, 255, 255, 0) );

            using( var spriteBatch = new SpriteBatch( this.graphicsDevice ) )
            {
                DrawEntries( atlas, spriteBatch );
            }

            this.atlas = null;
            this.graphicsDevice.SetRenderTarget( null );

            renderTarget.Name = configuration.OutputTextureName;
            return renderTarget;
        }

        /// <summary>
        /// Draws the TextureAtlasEntries of the given TextureAtlas using the given SpriteBatch.
        /// </summary>
        /// <param name="atlas">
        /// The TextureAtlas whose entries should be drawn.
        /// </param>
        /// <param name="spriteBatch">
        /// The SpriteBatch that should be used to draw the individual entries.
        /// </param>
        private static void DrawEntries( TextureAtlas atlas, SpriteBatch spriteBatch )
        {
            spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.Opaque );
            {
                foreach( TextureAtlasEntry entry in atlas.Entries )
                {
                    spriteBatch.Draw(
                        entry.Texture,
                        new Microsoft.Xna.Framework.Vector2( entry.Placement.X, entry.Placement.Y ),
                        Color.White
                    );
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Creates the RenderTarget2D that is used to render the TextureAtlas.
        /// </summary>
        /// <returns>
        /// The newly created RenderTarget2D.
        /// </returns>
        private RenderTarget2D CreateRenderTarget()
        {
            return new RenderTarget2D(
                this.graphicsDevice,
                this.atlas.Width,
                this.atlas.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None
            );
        }

        /// <summary>
        /// The TextureAtlas that is currently beeing rendered by this AtlasRenderer.
        /// </summary>
        private TextureAtlas atlas;

        /// <summary>
        /// The Xna GraphicsDevice that is required for rendering operations.
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Provides a mechanism that allows to receive the Xna GraphicsDevice.
        /// </summary>
        private readonly IGraphicsDeviceService graphicsDeviceService;
    }
}
