// <copyright file="SpriteSheetData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetData class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the data of a <see cref="SpriteSheet"/> which is serialized to/from XML.
    /// </summary>
    [Serializable]
    [XmlRoot( "SpriteSheet" )]
    public struct SpriteSheetData
    {
        /// <summary>
        /// Gets or sets the name of the <see cref="SpriteSheet"/>.
        /// </summary>
        public string Name 
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the names of the ISprites in the <see cref="SpriteSheet"/>.
        /// </summary>
        public string[] SpriteNames 
        { 
            get;
            set;
        }
    }
}
