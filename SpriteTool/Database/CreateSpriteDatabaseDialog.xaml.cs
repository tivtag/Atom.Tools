// <copyright file="CreateSpriteDatabaseDialog.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Database.CreateSpriteDatabaseDialog class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Database
{
    using System;
    using System.Windows;
    using Atom.Math;
    using Atom.Tools.SpriteTool.Atlas;
    using Atom.Xna;
    using Ookii.Dialogs.Wpf;

    /// <summary>
    /// 
    /// </summary>
    public partial class CreateSpriteDatabaseDialog : Window
    {
        /// <summary>
        /// Gets the SameTextureSpriteDatabase that has been created.
        /// </summary>
        public SpriteDatabase SpriteDatabase
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the CreateSpriteDatabaseDialog class.
        /// </summary>
        public CreateSpriteDatabaseDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the user has clicked the 'Select Folder' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnSelectInputFolderButtonClicked( object sender, RoutedEventArgs e )
        {
            var dialog = new VistaFolderBrowserDialog() { 
            };            

            if( dialog.ShowDialog() == true )
            {
                this.atlasConfiguration.InputImageFolder = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Called when the user has clicked the 'Create' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnCreateButtonClicked( object sender, RoutedEventArgs e )
        {
            try
            {
                this.CreateDatabase();
                this.DialogResult = true;
            }
            catch( Exception exc )
            {
                MessageBox.Show( exc.Message );
                this.DialogResult = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateDatabase()
        {
            this.atlasConfiguration.PreserveSpriteProperties = "true";
            this.atlasConfiguration.SpriteOutputType = AtlasSpriteOutputType.Database;
            this.atlasConfiguration.Spacing = (int)this.numericUpDownSpacing.Value;
            this.atlasConfiguration.OutputTextureName = this.textBoxName.Text + ".png";
            this.atlasConfiguration.AtlasSize = (Point2)this.atlasSize.Value;

            // ToDo: Call Atlas Batcher/Generator
            MessageBox.Show( "Please use the SpriteTool.AtlasBatcher to generate the initial Sprite Database (.sdb)", "Not yet implemented" );
        }

        /// <summary>
        /// Called when the user has clicked the 'Cancel' button.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnCancelButtonClicked( object sender, RoutedEventArgs e )
        {
            this.DialogResult = false;
        }

        private readonly AtlasConfiguration atlasConfiguration = new AtlasConfiguration();
    }
}
