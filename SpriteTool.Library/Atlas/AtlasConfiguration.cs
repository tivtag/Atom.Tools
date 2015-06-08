// <copyright file="AtlasConfiguration.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.AtlasConfiguration class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    using System;
    using Atom.Math;
    using System.IO;

    /// <summary>
    /// Represents the input configuration settings for the Sprite Atlas creation procedure.
    /// This class can't be inherited.
    /// </summary>
    [Serializable]
    public sealed class AtlasConfiguration
    {
        /// <summary>
        /// Gets or sets the base directory that is recursively searched for image files.
        /// </summary>
        public string InputImageFolder 
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the texture into which the Atlas is rendered.
        /// </summary>
        public string OutputTextureName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the Atlas into which the images are inserted.
        /// </summary>
        public Point2 AtlasSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum extra spacing between the elements in the Atlas.
        /// </summary>
        public int Spacing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value that indicates how the sprites of the atlas
        /// should be saved.
        /// </summary>
        public AtlasSpriteOutputType SpriteOutputType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the folder to which the Sprites are output.
        /// </summary>
        public string SpriteOutputFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether sprite properties,
        /// such as Color, Scale, etc. are preserved from previously existing Sprite.
        /// </summary>
        public string PreserveSpriteProperties
        {
            get;
            set;
        }
    }
}
