// <copyright file="SpriteDatabaseViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteDatabaseViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using Atom.Wpf;
    using Atom.Xna;
    using System.Windows;
    using Atom.Tools.SpriteTool.Properties;
    using System.IO;

    /// <summary>
    /// Defines the ViewModel that wraps the <see cref="SpriteDatabase"/> class.
    /// </summary>
    public sealed partial class SpriteDatabaseViewModel : ViewModel<SpriteDatabase>
    {
        /// <summary>
        /// Raised when the currently <see cref="SelectedSprite"/> has changed.
        /// </summary>
        public event EventHandler SelectedSpriteChanged;

        /// <summary>
        /// Gets the currently selected SpriteViewModel or AnimatedSpriteViewModel.
        /// </summary>
        public object SelectedSprite
        {
            get
            {
                return this._selectedSprite;
            }

            private set
            {
                if( value == this.SelectedSprite )
                    return;

                this._selectedSprite = value;
                this.OnPropertyChanged( "SelectedSprite" );
                this.SelectedSpriteChanged.Raise( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Gets the name of the SpriteDatabase.
        /// </summary>
        public string Name
        {
            get 
            {
                return this.Model.Name;
            }
        }

        /// <summary>
        /// Gets the view over the list of <see cref="SpriteViewModel"/>s that are part of this SpriteDatabaseViewModel.
        /// </summary>
        public ListCollectionView Sprites
        {
            get
            {
                return this.spritesView;
            }
        }

        /// <summary>
        /// Gets the view over the list of <see cref="AnimatedSpriteViewModel"/>s that are part of this SpriteDatabaseViewModel.
        /// </summary>
        public ListCollectionView AnimatedSprites
        {
            get
            {
                return this.animatedSpritesView;
            }
        }

        /// <summary>
        /// Gets the <see cref="ISpriteSource"/> that exposes the Sprites
        /// the SpriteDatabase contains.
        /// </summary>
        public INormalSpriteSource SpriteSource
        {
            get
            {
                return this.spriteSource;
            }
        }

        public string FilterSprite
        {
            get
            {
                return this.filterSprite;
            }

            set
            {
                this.filterSprite = value;
                this.spritesView.Refresh();
            }
        }

        public string FilterAnimatedSprite
        {
            get
            {
                return this.filterAnimatedSprite;
            }

            set
            {
                this.filterAnimatedSprite = value;
                this.animatedSpritesView.Refresh();
            }
        }

        /// <summary>
        /// Initializes a new instance of the SpriteDatabaseViewModel class.
        /// </summary>
        /// <param name="model">
        /// The SpriteDatabase the new SpriteDatabaseViewModel wraps around.
        /// </param>
        public SpriteDatabaseViewModel( SpriteDatabase model )
            : base( model )
        {
            this.spriteLoader = new SpriteDatabase.SpriteLoader( model );
            this.spriteSource = new DatabaseSpriteSource( model );

            // Sprites:
            foreach( var sprite in model.Atlas.Sprites )
            {
                this.sprites.Add( new ReadOnlySpriteViewModel( sprite ) );
            }

            this.spritesView = new ListCollectionView( this.sprites );
            this.spritesView.Filter = this.ShowsSprite;
            
            // Animated Sprites:
            foreach( var sprite in model.AnimatedSprites )
            {
                this.animatedSprites.Add( new AnimatedSpriteViewModel( sprite, this.spriteSource, spriteLoader ) );
            }

            this.animatedSpritesView = new ListCollectionView( this.animatedSprites );
            this.animatedSpritesView.Filter = this.ShowsAnimatedSprite;

            this.spritesView.CurrentChanged += this.OnCurrentSpriteChanged;
            this.animatedSpritesView.CurrentChanged += this.OnCurrentAnimatedSpriteChanged;
            
            this.InitializeCommands();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal bool Save()
        {
            string path = string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                @"Content\Sprites\{0}{1}",
                this.Model.Name,
                SpriteDatabase.ReaderWriter.Extension
            );

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine( basePath, path );

            try
            {
                Directory.CreateDirectory( Path.GetDirectoryName( fullPath ) );

                Storage.StorageUtilities.SafeSaveToFile<SpriteDatabase>(
                    fullPath,
                    this.Model,
                    new SpriteDatabase.ReaderWriter(
                        new SpriteAtlas.ReaderWriter( new NullTexture2DLoader() )
                    )
                );
            }
            catch( Exception exc )
            {
                MessageBox.Show( exc.Message, string.Empty, MessageBoxButton.OK );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Called when a different Sprite has been selected in the sprites list view.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnCurrentSpriteChanged( object sender, EventArgs e )
        {
            var sprite = this.spritesView.CurrentItem;
            var animatedSprite = this.animatedSpritesView.CurrentItem;

            if( sprite != null )
            {
                this.SelectedSprite = sprite;
                this.animatedSpritesView.MoveCurrentToPosition( -1 );
            }
            else
            {
                if( animatedSprites == null )
                {
                    this.SelectedSprite = null;
                }
            }
        }

        /// <summary>
        /// Called when a different AnimatedSprite has been selected in the animatedSprites list view.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnCurrentAnimatedSpriteChanged( object sender, EventArgs e )
        {
            var sprite = this.spritesView.CurrentItem;
            var animatedSprite = this.animatedSpritesView.CurrentItem;

            if( animatedSprite != null )
            {
                this.SelectedSprite = animatedSprite;
                this.spritesView.MoveCurrentToPosition( -1 );
            }
            else
            {
                if( sprite == null )
                {
                    this.SelectedSprite = null;
                }
            }
        }

        /// <summary>
        /// Adds the given AnimatedSprite to the SpriteDatabase.
        /// </summary>
        /// <param name="sprite">
        /// The AnimatedSprite to add.
        /// </param>
        /// <returns>
        /// The AnimatedSpriteViewModel that wraps the given AnimatedSprite.
        /// </returns>
        public AnimatedSpriteViewModel AddAnimatedSprite( AnimatedSprite sprite )
        {
            var viewModel = new AnimatedSpriteViewModel( sprite, this.spriteSource, this.spriteLoader );

            this.Model.AnimatedSprites.Add( sprite );
            this.animatedSprites.Add( viewModel );

            return viewModel;
        }

        private bool ShowsSprite( object obj )
        {
            if( string.IsNullOrEmpty( filterSprite ) )
            {
                return true;
            }

            var sprite = (ReadOnlySpriteViewModel)obj;
            return sprite.Name.Contains( filterSprite, StringComparison.OrdinalIgnoreCase );
        }

        private bool ShowsAnimatedSprite( object obj )
        {
            if( string.IsNullOrEmpty( filterAnimatedSprite ) )
            {
                return true;
            }

            var sprite = (AnimatedSpriteViewModel)obj;
            return sprite.Name.Contains( filterAnimatedSprite, StringComparison.OrdinalIgnoreCase );
        }

        private string filterSprite, filterAnimatedSprite;
        
        /// <summary>
        /// The storage field of the <see cref="SelectedSprite"/> property.
        /// </summary>
        private object _selectedSprite;

        /// <summary>
        /// Exposes the Sprites that the SpriteDatabase contains.
        /// </summary>
        private readonly INormalSpriteSource spriteSource;
        
        /// <summary>
        /// Exposes the Sprites that the SpriteDatabase contains.
        /// </summary>
        private readonly INormalSpriteLoader spriteLoader;

        /// <summary>
        /// The view over the sprites collection.
        /// </summary>
        private readonly ListCollectionView spritesView;

        /// <summary>
        /// The collection of ReadOnlySpriteViewModels.
        /// </summary>
        private readonly ObservableCollection<ReadOnlySpriteViewModel> sprites = new ObservableCollection<ReadOnlySpriteViewModel>();

        /// <summary>
        /// The view over the animatedSprites collection.
        /// </summary>
        private readonly ListCollectionView animatedSpritesView;

        /// <summary>
        /// The collection of AnimatedSpriteViewModels.
        /// </summary>
        private readonly ObservableCollection<AnimatedSpriteViewModel> animatedSprites = new ObservableCollection<AnimatedSpriteViewModel>();

    }
}
