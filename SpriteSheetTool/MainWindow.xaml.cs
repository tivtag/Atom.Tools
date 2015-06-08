// <copyright file="MainWindow.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.MainWindow class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System;
    using System.Threading;
    using System.Windows;
    using Atom.Math;
    using Atom.Xna;

    /// <summary>
    /// Represents the main window of the SpriteSheet tool.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.DataContext = this.application;
            this.InitializeComponent();
            
            // Start the XNA rendering logic in a different thread.
            this.xnaThread = new Thread( this.RunXna ) { IsBackground = true, Name = "XNA" };
            this.xnaThread.Start( this.xnaFormHost.Child.Handle );

            // Events:
            this.Closing += (sender, e) => {
                lock( this.xnaApplication )
                {
                    this.xnaApplication.Exit();
                }
            };

            this.application.SpriteSheetChanged += this.OnSpriteSheetChanged;
        }

        /// <summary>
        /// Called when the current SpriteSheet has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The ChangedValue{SpriteSheetViewModel} that contains the event data.
        /// </param>
        private void OnSpriteSheetChanged( object sender, ChangedValue<SpriteSheetViewModel> e )
        {
            if( e.OldValue != null )
            {
                e.OldValue.SelectedSpriteChanged -= this.OnSelectedSpriteChanged;
            }

            if( e.NewValue != null )
            {
                e.NewValue.SelectedSpriteChanged += this.OnSelectedSpriteChanged;
                this.xnaApplication.ShowSheet( e.NewValue.Model );
                this.xnaApplication.ShowCurrentSprite( null, -1 );
            }
            else
            {
                this.xnaApplication.ShowSheet( null );
                this.xnaApplication.ShowCurrentSprite( null, -1 );
            }
        }

        /// <summary>
        /// Called when the currently selected ISprite has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnSelectedSpriteChanged( object sender, EventArgs e )
        {
            var sheet = this.application.SpriteSheet;
            this.xnaApplication.ShowCurrentSprite( sheet.SelectedSprite, sheet.SelectedSpriteIndex );
        }

        /// <summary>
        /// Initializes and starts-up the Xna game loop.
        /// </summary>
        /// <param name="obj">
        /// Holds the handle of the control into which Xna is drawing.
        /// </param>
        private void RunXna( object obj )
        {
            this.xnaApplication = new XnaApplication( (IntPtr)obj );
            this.application.XnaApplication = this.xnaApplication;
            this.xnaApplication.Run();
        }

        /// <summary>
        /// Called when the user has clicked on the xna draw area.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The MouseEventArgs that contain the event data.
        /// </param>
        private void OnPictureBoxClicked( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            var sheetViewModel = this.application.SpriteSheet;
            if( sheetViewModel == null )
                return;

            var sheet = sheetViewModel.Model;

            switch( e.Button )
            {
                case System.Windows.Forms.MouseButtons.Left:
                    {
                        int index;
                        ISprite sprite = this.xnaApplication.GetSpriteAt( e.X, e.Y, out index );
                        if( index < 0 || index >= sheet.Count )
                            return;

                        sheetViewModel.Sprites.MoveCurrentToPosition( index );
                        this.listBoxSprites.ScrollIntoView( sheetViewModel.SelectedSprite );
                    }
                    break;

                case System.Windows.Forms.MouseButtons.Right:
                    {
                        int index;
                        ISprite sprite = this.xnaApplication.GetSpriteAt( e.X, e.Y, out index );
                        if( index < 0 || index >= sheet.Count )
                            return;

                        sheetViewModel.ExchangeSelected( index );

                        sheetViewModel.Sprites.MoveCurrentToPosition( index );
                        this.listBoxSprites.ScrollIntoView( sheetViewModel.SelectedSprite );
                    }
                    break;

                default:
                    break;
            }
        }

        private void OnKeyDown( object sender, System.Windows.Input.KeyEventArgs e )
        {
            switch( e.Key )
            {
                case System.Windows.Input.Key.A:
                    xnaApplication.TranslateView( new Vector2( -25.0f, 0.0f ) );
                    break;
                case System.Windows.Input.Key.D:
                    xnaApplication.TranslateView( new Vector2( 25.0f, 0.0f ) );
                    break;
                case System.Windows.Input.Key.W:
                    xnaApplication.TranslateView( new Vector2( 0.0f, -25.0f ) );
                    break;
                case System.Windows.Input.Key.S:
                    xnaApplication.TranslateView( new Vector2( 0.0f, 25.0f ) );
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when the user has clicked the File -> Exit MenuItem.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnFileExitMenuItemClicked( object sender, RoutedEventArgs e )
        {
            Application.Current.Shutdown();
        }
        
        /// <summary>
        /// Manages the drawing of sprites in the 'view-box' using XNA.
        /// </summary>
        private XnaApplication xnaApplication;

        /// <summary>
        /// The <see cref="SpriteSheetToolApplicationViewModel"/> object.
        /// </summary>
        private readonly SpriteSheetToolApplicationViewModel application = new SpriteSheetToolApplicationViewModel( SpriteSheetToolApplication.Current );
        
        /// <summary>
        /// The thread the XNA drawing and updating logic runs in.
        /// </summary>
        private Thread xnaThread;
    }
}
