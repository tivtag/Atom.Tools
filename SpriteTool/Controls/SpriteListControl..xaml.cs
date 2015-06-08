// <copyright file="SpriteListControl.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteListControl class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows;
    using Atom.Xna;

    /// <summary>
    /// </summary>
    public partial class SpriteListControl : UserControl
    {
        /// <summary>
        /// Gets or sets the <see cref="SpriteDatabaseViewModel"/> whose sprites
        /// are visualized by this SpriteListControl.
        /// </summary>
        public SpriteDatabaseViewModel SpriteDatabase
        {
            get
            {
                return (SpriteDatabaseViewModel)this.DataContext;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SpriteListControl class.
        /// </summary>
        public SpriteListControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the user has pressed any key while focusing the Animated Sprites list box.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The KeyEventArgs that contain the event data.
        /// </param>
        private void OnAnimatedSpritesListBoxKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Delete )
            {
                var database = this.SpriteDatabase;
                if( database == null )
                    return;

                database.RemoveSelectedAnimatedSprite.Execute( null );
            }
        }

        private void OnCloneAnimatedSpriteClicked( object sender, System.Windows.RoutedEventArgs e )
        {
            var sprite = ((AnimatedSpriteViewModel)((FrameworkElement)sender).DataContext).Model;

            AnimatedSprite clone = sprite.Clone();
            clone.Name += "_2";

            this.SpriteDatabase.AddAnimatedSprite( clone );
        }
    }
}
