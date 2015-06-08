// <copyright file="SpriteSheetToolApplication.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteSheetToolApplication class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System.Windows;

    /// <summary>
    /// The SpriteSheet Tool allows the creation of Sprite Sheets.
    /// </summary>
    public sealed partial class SpriteSheetToolApplication : Application
    {
        /// <summary>
        /// Gets the currently active SpriteSheetToolApplication.
        /// </summary>
        public static new SpriteSheetToolApplication Current
        {
            get
            {
                return (SpriteSheetToolApplication)Application.Current;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SpriteSheetToolApplication class.
        /// </summary>
        public SpriteSheetToolApplication()
        {
        }
    }
}
