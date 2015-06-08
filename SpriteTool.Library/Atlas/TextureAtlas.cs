// <copyright file="TextureAtlas.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.TextureAtlas class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Atom.Math;

    /// <summary>
    /// Represents an atlas of textures.
    /// </summary>
    public sealed class TextureAtlas
    {
        /// <summary>
        /// Gets the width of this TextureAtlas.
        /// </summary>
        public int Width
        {
            get
            {
                return this.size.X;
            }
        }

        /// <summary>
        /// Gets the height of this TextureAtlas.
        /// </summary>
        public int Height
        {
            get
            {
                return this.size.Y;
            }
        }

        /// <summary>
        /// Gets the TextureAtlasEntries that are part of this TextureAtlas.
        /// </summary>
        public IEnumerable<TextureAtlasEntry> Entries
        {
            get
            {
                return this.entries;
            }
        }

        /// <summary>
        /// Initializes a new instance of the TextureAtlas class.
        /// </summary>
        /// <param name="size">
        /// The size of the new TextureAtlas.
        /// </param>
        /// <param name="initialCapacity">
        /// The initial capacity of the new TextureAtlas.
        /// </param>
        public TextureAtlas( Point2 size, int initialCapacity )
        {
            Contract.Requires<ArgumentException>( size.X > 0 );
            Contract.Requires<ArgumentException>( size.Y > 0 );

            this.size = size;
            this.entries = new List<TextureAtlasEntry>( initialCapacity );
        }

        /// <summary>
        /// Adds the given TextureAtlasEntry to this TextureAtlas.
        /// </summary>
        /// <param name="entry">
        /// The TextureAtlasEntry to add.
        /// </param>
        public void AddEntry( TextureAtlasEntry entry )
        {
            Contract.Requires<ArgumentNullException>( entry != null );
            Contract.Requires<ArgumentException>( (entry.Placement.X + entry.Texture.Width) <= this.Width );
            Contract.Requires<ArgumentException>( (entry.Placement.Y + entry.Texture.Height) <= this.Height );

            this.entries.Add( entry );
        }

        /// <summary>
        /// Gets the TextureAtlasEntry that are part of this TextureAtlas.
        /// </summary>
        /// <returns>
        /// The TextureAtlasEntry that are part of this TextureAtlas.
        /// </returns>
        public IEnumerable<TextureAtlasEntry> GetEntries()
        {
            return this.entries.AsReadOnly();
        }

        /// <summary>
        /// The size of this TextureAtlas.
        /// </summary>
        private readonly Point2 size;

        /// <summary>
        /// The entries that are part of this TextureAtlas.
        /// </summary>
        private readonly List<TextureAtlasEntry> entries;
    }
}
