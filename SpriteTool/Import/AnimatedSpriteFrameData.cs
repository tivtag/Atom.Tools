// <copyright file="AnimatedSpriteFrameData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines theAtom.Tools.SpriteTool.Import.AnimatedSpriteFrameData structure.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Import
{
    using System;
    using System.Xml.Serialization;
    using Atom.Math;

    /// <summary>
    /// Defines the data of a single <see cref="AnimatedSpriteFrame"/> in a XML file.
    /// </summary>
    [Serializable]
    [XmlType( TypeName="Frame" )]
    public struct AnimatedSpriteFrameData
    {
        /// <summary>
        /// Gets or sets the name of the Sprite to display in this AnimatedSpriteFrame.
        /// </summary>
        public string SpriteName { get; set; }

        /// <summary>
        /// Gets or sets the rendering offset to apply when rendering this AnimatedSpriteFrame.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the time this AnimatedSpriteFrame lasts.
        /// </summary>
        public float Time { get; set; }
    }

}
