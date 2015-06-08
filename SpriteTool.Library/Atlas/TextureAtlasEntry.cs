// <copyright file="TextureAtlasEntry.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.TextureAtlasEntry class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom.Math;
    using Microsoft.Xna.Framework.Graphics;
    
    /// <summary>
    /// Represents an entry of a <see cref="TextureAtlas"/>.
    /// </summary>
    public sealed class TextureAtlasEntry
    {
        /// <summary> 
        /// Gets the texture that is assigned to this TextureAtlasEntry.
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }
        }

        /// <summary>
        /// Gets the placement of the texture in the TextureAtlas.
        /// </summary>
        public Point2 Placement
        {
            get
            {
                return this.placement;
            }
        }

        /// <summary>
        /// Initializes a new instance of the TextureAtlasEntry class.
        /// </summary>
        /// <param name="texture">
        /// The texture of the new TextureAtlasEntry.
        /// </param>
        /// <param name="placement">
        /// The placement of the texture in the TextureAtlas.
        /// </param>
        public TextureAtlasEntry( Texture2D texture, Point2 placement )
        {
            Contract.Requires<ArgumentNullException>( texture != null );
            Contract.Requires<ArgumentException>( placement.X >= 0 );
            Contract.Requires<ArgumentException>( placement.Y >= 0 );

            this.texture = texture;
            this.placement = placement;
        }

        /// <summary>
        /// The placement of the texture in the TextureAtlas.
        /// </summary>
        private readonly Point2 placement;

        /// <summary>
        /// The texture that is assigned to this TextureAtlasEntry.
        /// </summary>
        private readonly Texture2D texture;
    }
}
