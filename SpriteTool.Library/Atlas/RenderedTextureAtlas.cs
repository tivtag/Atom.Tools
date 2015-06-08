// <copyright file="RenderedTextureAtlas.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.RenderedTextureAtlas class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents a <see cref="TextureAtlas"/> that has been rendered into a single Texture2D.
    /// This class can't be inherited.
    /// </summary>
    public sealed class RenderedTextureAtlas
    {
        /// <summary>
        /// Gets the <see cref="TextureAtlas"/> this RenderedTextureAtlas is based on.
        /// </summary>
        public TextureAtlas Atlas
        {
            get
            {
                return this.atlas;
            }
        }

        /// <summary>
        /// Gets the <see cref="Texture2D"/> that contains the rendered TextureAtlas.
        /// </summary>
        public Texture2D AtlasTexture
        {
            get
            {
                return this.atlasTexture;
            }
        }

        /// <summary>
        /// Gets the <see cref="AtlasConfiguration"/> under which this RenderedTextureAtlas was created.
        /// </summary>
        public AtlasConfiguration Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        /// <summary>
        /// Gets the width of the atlas texture resource, in pixels.
        /// </summary>
        public int Width
        {
            get
            {
                return this.atlasTexture.Width;
            }
        }

        /// <summary>
        /// Gets the height of the atlas texture resource, in pixels.
        /// </summary>
        public int Height
        {
            get
            {
                return this.atlasTexture.Height;
            }
        }

        /// <summary>
        /// Initializes a new instance of the RenderedTextureAtlas class.
        /// </summary>
        /// <param name="atlas">
        /// The <see cref="TextureAtlas"/> this RenderedTextureAtlas is based on.
        /// </param>
        /// <param name="atlasTexture">
        /// The <see cref="Texture2D"/> that contains the rendered TextureAtlas.
        /// </param>
        /// <param name="configuration">
        /// The <see cref="AtlasConfiguration"/> under which this RenderedTextureAtlas was created.
        /// </param>
        public RenderedTextureAtlas( TextureAtlas atlas, Texture2D atlasTexture, AtlasConfiguration configuration )
        {
            Contract.Requires<ArgumentNullException>( atlas != null );
            Contract.Requires<ArgumentNullException>( atlasTexture != null );
            Contract.Requires<ArgumentNullException>( configuration != null );

            this.atlas = atlas;
            this.atlasTexture = atlasTexture;
            this.configuration = configuration;
        }

        /// <summary>
        /// The <see cref="TextureAtlas"/> this RenderedTextureAtlas is based on.
        /// </summary>
        private readonly TextureAtlas atlas;

        /// <summary>
        /// The <see cref="Texture2D"/> that contains the rendered TextureAtlas.
        /// </summary>
        private readonly Texture2D atlasTexture;

        /// <summary>
        /// The <see cref="AtlasConfiguration"/> under which this RenderedTextureAtlas was created.
        /// </summary>
        private readonly AtlasConfiguration configuration;
    }
}
