// <copyright file="SpriteSheetToolApplicationViewModel.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.SpriteSheetToolApplicationViewModel class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System;
    using System.Windows.Input;
    using Atom.Wpf;
    using Atom.Xna;
    using System.Windows;

    /// <summary>
    /// The main view-model of the SpriteSheet Tool.
    /// </summary>
    public sealed partial class SpriteSheetToolApplicationViewModel : ViewModel<SpriteSheetToolApplication>
    {
        /// <summary>
        /// Raised when the current <see cref="SpriteSheet"/> has changed.
        /// </summary>
        public event RelaxedEventHandler<ChangedValue<SpriteSheetViewModel>> SpriteSheetChanged;

        /// <summary>
        /// Gets the <see cref="SpriteSheetViewModel"/> that is currently modified by the SpriteSheet tool.
        /// </summary>
        public SpriteSheetViewModel SpriteSheet
        {
            get
            {
                return this.sheet;
            }

            private set
            {
                if( value == this.SpriteSheet )
                    return;

                var oldSheet = this.sheet;
                this.sheet = value;

                this.OnPropertyChanged( "SpriteSheet" );
                this.SpriteSheetChanged.Raise( this, new ChangedValue<SpriteSheetViewModel>( oldSheet, value ) );
            }
        }
        
        /// <summary>
        /// Gets the <see cref="ISpriteLoader"/> that provides a mechanism
        /// for loading <see cref="ISprite"/> assets.
        /// </summary>
        public ISpriteLoader SpriteLoader
        {
            get
            {
                return this.XnaApplication.SpriteLoader;
            }
        }

        /// <summary>
        /// Gets the <see cref="ISpriteSource"/> that provides access
        /// to all ISprite assets that may be added to a SpriteSheet.
        /// </summary>
        public ISpriteSource SpriteSource
        {
            get
            {
                return this.XnaApplication.SpriteLoader;
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
        /// Initializes a new instance of the SpriteSheetToolApplicationViewModel class.
        /// </summary>
        /// <param name="model">
        /// The SpriteSheetToolApplication the new SpriteSheetToolApplicationViewModel wraps around.
        /// </param>
        public SpriteSheetToolApplicationViewModel( SpriteSheetToolApplication model )
            : base( model )
        {
            this.InitializeCommands();
        }

        /// <summary>
        /// Represents the storage field of the <see cref="SpriteSheet"/> property.
        /// </summary>
        private SpriteSheetViewModel sheet;
    }
}
