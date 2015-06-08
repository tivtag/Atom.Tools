// <copyright file="AnimatedSpriteData.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines theAtom.Tools.SpriteTool.Import.AnimatedSpriteData class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Import
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the data of a AnimatedSprite which is saved to the disc.
    /// </summary>
    [Serializable]
    [XmlRoot( ElementName="AnimatedSprite" )]
    public sealed class AnimatedSpriteData
    {
        /// <summary>
        /// Gets or sets the name of the AnimatedSprite.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether instances of the AnimatedSprite are looping by default.
        /// </summary>
        public bool IsLoopingByDefault { get; set; }

        /// <summary>
        /// Gets or sets the default animation speed of instances of the AnimatedSprite.
        /// </summary>
        public float DefaultAnimationSpeed { get; set; }

        /// <summary>
        /// Gets or sets the data of all frames of the AnimatedSprite.
        /// </summary>
        public AnimatedSpriteFrameData[] Frames { get; set; }
    }
}
