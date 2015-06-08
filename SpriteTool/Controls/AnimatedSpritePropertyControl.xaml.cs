// <copyright file="AnimatedSpritePropertyControl.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.AnimatedSpritePropertyControl class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System.Windows.Controls;

    /// <summary>
    /// 
    /// </summary>
    public partial class AnimatedSpritePropertyControl : UserControl
    {
        /// <summary>
        /// Gets or sets the AnimatedSpriteViewModel this AnimatedSpritePropertyControl has been bound to.
        /// </summary>
        public AnimatedSpriteViewModel AnimatedSpriteViewModel
        {
            get
            {
                return (AnimatedSpriteViewModel)this.DataContext;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the AnimatedSpritePropertyControl class.
        /// </summary>
        public AnimatedSpritePropertyControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the user presses any key while the frames ListBox has focus.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The KeyEventArgs that contain the event data.
        /// </param>
        private void OnFramesListBoxKeyDown( object sender, System.Windows.Input.KeyEventArgs e )
        {
            if( e.Key == System.Windows.Input.Key.Delete )
            {
                this.AnimatedSpriteViewModel.RemoveSelectedFrame.Execute( null );
            }
        }
    }
}
