// <copyright file="ReadOnlySpriteViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.ReadOnlySpriteViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using Atom.Xna;
    using Atom.Wpf;

    /// <summary>
    /// 
    /// </summary>
    public sealed class ReadOnlySpriteViewModel : ViewModel<Sprite>, IViewModel<ISprite>
    {
        /// <summary>
        /// Gets the name of the Sprite.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Model.Name;
            }
        }

        /// <summary>
        /// Gets the ISprite this ReadOnlySpriteViewModel wraps around.
        /// </summary>
        ISprite IViewModel<ISprite>.Model
        {
            get
            {
                return this.Model;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ReadOnlySpriteViewModel class.
        /// </summary>
        /// <param name="model"></param>
        public ReadOnlySpriteViewModel( Sprite model )
            : base( model )
        {
        }
    }
}
