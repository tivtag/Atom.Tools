// <copyright file="SpriteSheetViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Data;
    using Atom.Collections;
    using Atom.Wpf;
    using Atom.Xna;

    /// <summary>
    /// Represents the ViewModel that provides data binding and command support
    /// for interacting with a SpriteSheet.
    /// </summary>
    public sealed partial class SpriteSheetViewModel : ViewModel<SpriteSheet>
    {
        /// <summary>
        /// Raised when the currently <see cref="SelectedSprite"/> has changed.
        /// </summary>
        public event EventHandler SelectedSpriteChanged;

        /// <summary>
        /// Gets or sets the name that uniquely identifies the SpriteSheet.
        /// </summary>
        public string Name
        {
            get
            {
                return this.Model.Name;
            }

            set
            {
                if( value == this.Name )
                    return;

                this.Model.Name = value;
                this.OnPropertyChanged( "Name" );
            }
        }

        /// <summary>
        /// Gets the currently selected <see cref="ISprite"/>.
        /// </summary>
        public ISprite SelectedSprite
        {
            get
            {
                return this.spritesView.CurrentItem as ISprite;
            }
        }

        /// <summary>
        /// Gets the zero-based index of the currently <see cref="SelectedSprite"/>
        /// in the SpriteSheet.
        /// </summary>
        public int SelectedSpriteIndex
        {
            get
            {
                return this.spritesView.CurrentPosition;
            }
        }

        /// <summary>
        /// Gets the view over the list of ISprites that are part
        /// of the SpriteSheet.
        /// </summary>
        public ListCollectionView Sprites
        {
            get
            {
                return this.spritesView;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SpriteSheetViewModel class.
        /// </summary>
        /// <param name="model">
        /// The SpriteSheet the new SpriteSheetViewModel wraps around.
        /// </param>
        /// <param name="spriteSource">
        /// Provides access to the ISprites that may be added to the SpriteSheet.
        /// </param>
        public SpriteSheetViewModel( SpriteSheet model, ISpriteSource spriteSource )
            : base( model )
        {
            foreach( ISprite sprite in model )
            {
                this.sprites.Add( sprite );
            }

            this.spritesView = new ListCollectionView( this.sprites );
            this.spritesView.CurrentChanged += (sender, e) => this.SelectedSpriteChanged.Raise( this );

            this.InitializeCommands( spriteSource );
        }

        /// <summary>
        /// Exchanges the currently selected ISprite
        /// with the ISprite at the given zero-based index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the other ISprite.
        /// </param>
        public void ExchangeSelected( int index )
        {
            if( this.SelectedSprite == null )
                return;

            int indexSelected = this.spritesView.CurrentPosition;
            if( index == 0 || indexSelected == 0 )
                return;
            
            // Exchange:
            this.Model.SwapItems( index, indexSelected );
            this.sprites.SwapItems( index, indexSelected );

            // Notify
            this.spritesView.Refresh();
        }

        /// <summary>
        /// The view over the sprites collection.
        /// </summary>
        private readonly ListCollectionView spritesView;

        /// <summary>
        /// The obsevable list of ISprites that are part of the SpriteSheet.
        /// </summary>
        private readonly ObservableCollection<ISprite> sprites = new ObservableCollection<ISprite>();
    }
}
