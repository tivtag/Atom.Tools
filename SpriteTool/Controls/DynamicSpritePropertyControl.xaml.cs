// <copyright file="DynamicSpritePropertyControl.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.DynamicSpritePropertyControl class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System.Windows.Controls;
    using Atom.Wpf;
    using Atom.Xna;

    /// <summary>
    /// 
    /// </summary>
    public partial class DynamicSpritePropertyControl : UserControl
    {
        /// <summary>
        /// Gets or sets the current control displayed in this DynamicSpritePropertyControl. 
        /// </summary>
        private Control Control
        {
            get
            {
                return this._control;
            }

            set
            {
                if( value == this.Control )
                    return;
                
                this._control = value;
                this.container.Children.Clear();

                if( value != null )
                {
                    this.container.Children.Add( value );
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the DynamicSpritePropertyControl class.
        /// </summary>
        public DynamicSpritePropertyControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Shows a control that allows editing the given IViewModel<ISprite>
        /// in this DynamicSpritePropertyControl.
        /// </summary>
        /// <param name="animatedSpriteViewModel">
        /// The Sprite to visualize.
        /// </param>
        public void ShowSprite( IViewModel<ISprite> spriteViewModel )
        {
            var readOnlySprite = spriteViewModel as ReadOnlySpriteViewModel;

            if( readOnlySprite != null )
            {
                this.ShowSprite( readOnlySprite );
                return;
            }

            var animatedSprite = spriteViewModel as AnimatedSpriteViewModel;

            if( animatedSprite != null )
            {
                this.ShowSprite( animatedSprite );
                return;
            }

            this.Control = null;
        }

        /// <summary>
        /// Shows a control that allows editing the given AnimatedSpriteViewModel
        /// in this DynamicSpritePropertyControl.
        /// </summary>
        /// <param name="animatedSpriteViewModel">
        /// The Sprite to visualize.
        /// </param>
        private void ShowSprite( AnimatedSpriteViewModel animatedSpriteViewModel )
        {
            this.animatedSpriteControl.AnimatedSpriteViewModel = animatedSpriteViewModel;
            this.Control = this.animatedSpriteControl;
        }

        /// <summary>
        /// Shows a control that allows editing the given ReadOnlySpriteViewModel
        /// in this DynamicSpritePropertyControl.
        /// </summary>
        /// <param name="spriteViewModel">
        /// The Sprite to visualize.
        /// </param>
        private void ShowSprite( ReadOnlySpriteViewModel spriteViewModel )
        {
            this.Control = null;
        }

        /// <summary>
        /// Represents the storage field of the Control property.
        /// </summary>
        private Control _control;

        /// <summary>
        /// The control that is used to manipulate an AnimatedSpriteViewModel.
        /// </summary>
        private readonly AnimatedSpritePropertyControl animatedSpriteControl = new AnimatedSpritePropertyControl();
    }
}
