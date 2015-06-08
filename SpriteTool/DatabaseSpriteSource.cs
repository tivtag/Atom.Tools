// <copyright file="DatabaseSpriteSource.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.DatabaseSpriteSource class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.Collections.Generic;
    using Atom.Xna;

    /// <summary>
    /// Implements an <see cref="ISpriteSource"/> that pulls the sprites
    /// from a <see cref="SpriteDatabase"/>.
    /// </summary>
    public sealed class DatabaseSpriteSource : INormalSpriteSource
    {
        /// <summary>
        /// Gets the <see cref="Sprite"/>s that the SpriteDatabase contains.
        /// </summary>
        public IEnumerable<Sprite> Sprites
        {
            get
            {
                return this.database.Atlas.Sprites;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseSpriteSource class.
        /// </summary>
        /// <param name="database">
        /// The database the new DatabaseSpriteSource wraps around.
        /// </param>
        public DatabaseSpriteSource( SpriteDatabase database )
        {
            if( database == null )
                throw new ArgumentNullException( "database" );

            this.database = database;
        }

        /// <summary>
        /// The SpriteDatabase whose Sprites are exposed by this DatabaseSpriteSource.
        /// </summary>
        private readonly SpriteDatabase database;
    }
}
