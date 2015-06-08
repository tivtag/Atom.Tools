// <copyright file="SpriteToolApplicationViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteToolApplicationViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Atom.Tools.SpriteTool.Properties;
    using Atom.Storage;
    using Atom.Wpf;
    using Atom.Xna;

    /// <summary>
    /// Represents the ViewModel of the SpriteToolApplication.
    /// </summary>
    public sealed partial class SpriteToolApplicationViewModel : ViewModel<SpriteToolApplication>
    {
        /// <summary>
        /// Raised when the current SpriteDatabase has been saved.
        /// </summary>
        public event EventHandler Saved;

        /// <summary>
        /// Raised when the current SpriteDatabase has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<SpriteDatabaseViewModel>> SpriteDatabaseChanged;
        
        /// <summary>
        /// Gets or sets the SpriteDatabaseViewModel that is currently modified by the user
        /// of the SpriteTool.
        /// </summary>
        public SpriteDatabaseViewModel SpriteDatabase
        {
            get
            {
                return this._spriteDatabaseViewModel;
            }

            set
            {
                if( value == this.SpriteDatabase )
                    return;

                var oldValue = this._spriteDatabaseViewModel;
                this._spriteDatabaseViewModel = value;

                this.OnPropertyChanged( "SpriteDatabase" );
                this.SpriteDatabaseChanged.Raise( this, new ChangedValue<SpriteDatabaseViewModel>( oldValue, value ) );
            }
        }

        /// <summary>
        /// Gets or sets the XnaApplication object that provides
        /// access to xna-related services.
        /// </summary>
        public XnaApplication XnaApplication
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the ICommand that when executed
        /// saves the current SpriteDatabase.
        /// </summary>
        public ICommand Save
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the ICommand that when executed allows the user to
        /// import Animated Sprites that fit into the current SpriteDatabase.
        /// </summary>
        public ICommand ImportAnimatedSprites
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the ICommand that when executed adds a new AnimatedSprite
        /// to the current SpriteDatabase.
        /// </summary>
        public ICommand AddNewAnimatedSprite
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the ICommand that when executed allows the user to extract nXn-sized tiles from a bitmap.
        /// They are saved as individual tgas.
        /// </summary>
        public ICommand ExtractTiles
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Initializes a new instance of the SpriteToolApplication class.
        /// </summary>
        /// <param name="model">
        /// The SpriteToolApplication the new SpriteToolApplicationViewModel wraps around.
        /// </param>
        public SpriteToolApplicationViewModel( SpriteToolApplication model )
            : base( model )
        {
            this.Save = new SaveCommand( this );
            this.AddNewAnimatedSprite = new AddNewAnimatedSpriteCommand( this );
            this.ImportAnimatedSprites = new ImportAnimatedSpritesCommand( this );
            this.ExtractTiles = new ExtractTilesCommand( this );
        }

        /// <summary>
        /// Saves the current SpriteDatabase.
        /// </summary>
        private void SaveDatabase()
        {
            if( this.SpriteDatabase == null )
                return;

            if( this.SpriteDatabase.Save() )
            {
                if( Settings.Default.LastSaved == null )
                    Settings.Default.LastSaved = new System.Collections.Specialized.StringCollection();

                var lastSaved = Settings.Default.LastSaved;

                lastSaved.Remove( this.SpriteDatabase.Name );
                lastSaved.Add( this.SpriteDatabase.Name );
                Settings.Default.Save();

                this.Saved.Raise( this, EventArgs.Empty );
            }
        }

        /// <summary>
        /// Attempts to open the SpriteDatabase at the given path.
        /// </summary>
        /// <param name="path">
        /// The path to the .sdb file.
        /// </param>
        public void OpenDatabase( string path )
        {
            SafeExecute.WithMsgBox( () => {
                var database = StorageUtilities.LoadFromFile<SpriteDatabase>(
                    path,
                    new SpriteDatabase.ReaderWriter( this.XnaApplication.TextureLoader )
                );

                this.SpriteDatabase = new SpriteDatabaseViewModel( database );
            } );
        }

        /// <summary>
        /// Represents the storage field of the SpriteDatabase property.
        /// </summary>
        private SpriteDatabaseViewModel _spriteDatabaseViewModel;
    }
}
