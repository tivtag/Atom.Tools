// <copyright file="AtlasSpriteOutputType.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.AtlasSpriteOutputType enumeration.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    /// <summary>
    /// Enumerates the different ways the sprites of a Sprite Atlas are saved.
    /// </summary>
    public enum AtlasSpriteOutputType
    {
        /// <summary>
        /// The sprites the atlas contains are saved as individual '.spr' files.
        /// </summary>
        IndividualSprites,

        /// <summary>
        /// The sprites are thrown into a single sprite database file.
        /// </summary>
        Database
    }
}
