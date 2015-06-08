// <copyright file="MainWindow.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.MainWindow class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Atom.Math;
    using Atom.Tools.SpriteTool.Properties;
    using Atom.Wpf;
    using Atom.Xna;

    /// <summary>
    /// Represents the main window of the SpriteTool application.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Gets the SpriteDatabaseViewModel that is currently modified by the user
        /// of the SpriteTool.
        /// </summary>
        public SpriteDatabaseViewModel SpriteDatabase
        {
            get
            {
                return this.application.SpriteDatabase;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.DataContext = this.application;
            this.InitializeComponent();
            this.BuildQuickOpenList();
                       
            // Start the XNA rendering logic in a different thread.
            this.xnaThread = new Thread( this.RunXna ) { IsBackground = true, Name = "XNA" };
            this.xnaThread.Start( this.xnaFormHost.Child.Handle );

            // Events:
            this.Closing += (sender, e) => {
                this.xnaApplication.Exit();
            };

            this.application.SpriteDatabaseChanged += this.OnSpriteDatabaseChanged;
            this.application.Saved += (sender, e) => { this.BuildQuickOpenList(); };
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
        /// Called when the current SpriteDatabase has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The ChangedValue{SpriteDatabaseViewModel} that contains the event data.
        /// </param>
        private void OnSpriteDatabaseChanged( object sender, ChangedValue<SpriteDatabaseViewModel> e )
        {
            if( e.OldValue != null )
            {
                e.OldValue.SelectedSpriteChanged -= this.OnSelectedSpriteChanged;
            }

            this.spriteListControl.SpriteDatabase = e.NewValue;

            if( e.NewValue != null )
            {
                e.NewValue.SelectedSpriteChanged += this.OnSelectedSpriteChanged;
                this.ShowSelectedSprite();
            }
            else
            {
                this.xnaApplication.ShowSprite( null );
            }
        }

        /// <summary>
        /// Called when the user has clicked the 'New Sprite Database' menu item.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnNewSpriteDatabaseMenuClicked( object sender, RoutedEventArgs e )
        {
            var dialog = new Atom.Tools.SpriteTool.Database.CreateSpriteDatabaseDialog();

            if( dialog.ShowDialog() == true )
            {
            }
        }

        /// <summary>
        /// Called when the user has clicked the 'New Sprite Database' menu item.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnOpenSpriteDatabaseMenuClicked( object sender, RoutedEventArgs e )
        {
            var dialog = new Microsoft.Win32.OpenFileDialog() {
                InitialDirectory = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Content\\Sprites\\" ),
                Filter = "Sprite Database (*.sdb)|*.sdb",
                RestoreDirectory = true 
            };

            if( dialog.ShowDialog() == true )
            {
                this.application.OpenDatabase( dialog.FileName );
            }
        }
     
        /// <summary>
        /// Called when the currently selected sprite has changed.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The EventArgs that contain the event data.
        /// </param>
        private void OnSelectedSpriteChanged( object sender, EventArgs e )
        {
            this.ShowSelectedSprite();
        }

        /// <summary>
        /// Shows the currently selected sprite.
        /// </summary>
        private void ShowSelectedSprite()
        {
            IViewModel<ISprite> spriteViewModel = this.SpriteDatabase.SelectedSprite as IViewModel<ISprite>;
            ISprite sprite = spriteViewModel != null ? spriteViewModel.Model : null;

            this.spritePropertyControl.ShowSprite( spriteViewModel );
            this.xnaApplication.ShowSprite( sprite );
        }

        /// <summary>
        /// Gets called when the user presses any key while this MainWindow is focused.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The KeyEventArgs that contain the event data.
        /// </param>
        private void OnKeyDown( object sender, KeyEventArgs e )
        {
            switch( e.Key )
            {
                case Key.R:
                    this.xnaApplication.ResetView();
                    break;

                case Key.A:
                    if( e.KeyboardDevice.IsKeyDown( Key.LeftCtrl ) )
                    {
                        if( this.SpriteDatabase == null )
                            return;

                        this.SpriteDatabase.AddNewAnimatedSprite.Execute( null );
                    }

                    break;

                case Key.S:
                    if( e.KeyboardDevice.IsKeyDown( Key.LeftCtrl ) )
                    {
                        this.application.Save.Execute( null );
                    }

                    break;

                case Key.Space:
                    this.xnaApplication.DesignMode = !this.xnaApplication.DesignMode;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Gets called when the user moves the mouse over the 'view-field'.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The MouseEventArgs that contain the event data.
        /// </param>
        private void OnPictureBoxMouseMove( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            if( this.isCapturingMousePosition )
            {
                var position = e.Location;
                Vector2 moveOffset = new Vector2( 
                    position.X - lastMousePosition.X,
                    position.Y - lastMousePosition.Y 
                );

                switch( this.activeViewChangeType )
                {
                    case ViewChangeType.Translate:
                        this.xnaApplication.TranslateView( moveOffset );
                        break;

                    case ViewChangeType.Zoom:
                        float length = moveOffset.Length;
                        float factor = length / 250.0f * System.Math.Sign( -moveOffset.Y );

                        this.xnaApplication.ZoomView( factor );
                        break;

                    default:
                        break;
                }

                this.lastMousePosition = position;
            }
        }

        /// <summary>
        /// Gets called when the user presses the mouse over the 'view-field'.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The MouseEventArgs that contain the event data.
        /// </param>
        private void OnPictureBoxMouseDown( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            if( this.isCapturingMousePosition )
                return;

            this.isCapturingMousePosition = true;
            this.lastMousePosition        = e.Location;

            switch( e.Button )
            {
                case System.Windows.Forms.MouseButtons.Left:
                    this.activeViewChangeType = ViewChangeType.Translate;
                    break;

                case System.Windows.Forms.MouseButtons.Right:
                    this.activeViewChangeType = ViewChangeType.Zoom;
                    break;

                default:
                    this.activeViewChangeType = ViewChangeType.None;
                    break;
            }
        }

        /// <summary>
        /// Gets called when the user releases the mouse over the 'view-field'.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The MouseEventArgs that contain the event data.
        /// </param>
        private void OnPictureBoxMouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            this.activeViewChangeType = ViewChangeType.None;
            this.isCapturingMousePosition = false;
        }

        /// <summary>
        /// Gets called when the user presses on the Reset button of the View menu.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnViewResetMenuClickClicked( object sender, RoutedEventArgs e )
        {
            this.xnaApplication.ResetView();
        }

        /// <summary>
        /// Builds the quick-open list.
        /// </summary>
        private void BuildQuickOpenList()
        {
            var menuItems = this.menuItemOpenRecent.Items;
            menuItems.Clear();

            var lastSaved = Settings.Default.LastSaved;
            if( lastSaved != null )
            {
                for( int index = lastSaved.Count - 1; index >= 0; --index )
                {
                    string name = lastSaved[index];

                    var menuItem = new MenuItem()
                    {
                        Header = name,
                        Tag = name
                    };

                    menuItem.Click += this.OnQuickOpenMenuItemClicked;
                    menuItems.Add( menuItem );
                }
            }

            this.menuItemOpenRecent.IsEnabled = menuItems.Count > 0;
            this.menuItemOpenRecent.Visibility = 
                this.menuItemOpenRecent.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Called when the user clicks on a quick open menu items.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnQuickOpenMenuItemClicked( object sender, RoutedEventArgs e )
        {
            MenuItem menuItem = (MenuItem)sender;
            string databaseName = (string)menuItem.Tag;

            string path = @"Content\Sprites\" + databaseName + ".sdb";
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine( basePath, path );

            this.application.OpenDatabase( fullPath );
        }

        /// <summary>
        /// Called when the user clicks on the Exit MenuItem.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The RoutedEventArgs that contain the event data.
        /// </param>
        private void OnExitMenuItemClicked( object sender, RoutedEventArgs e )
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Gets called when the user uses the mouse wheel when over the main window.
        /// This is a work-around over that its not possible to capture mouse wheel input
        /// pver the WinForms 'view-field'.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The MouseWheelEventArgs that contain the event data.
        /// </param>
        private void OnMouseWheel( object sender, MouseWheelEventArgs e )
        {
            float factor = e.Delta / 1000.0f;
            this.xnaApplication.ZoomView( factor );
        }

        /// <summary>
        /// Enumerates the view change types supported by the application.
        /// </summary>
        private enum ViewChangeType
        {
            None,
            Translate,
            Zoom 
        };

        /// <summary>
        /// Specifies whether the application is currenttly capturing 
        /// the current mouse position within the 'view-box'.
        /// </summary>
        private bool isCapturingMousePosition;

        /// <summary>
        /// Specifies the currently active <see cref="ViewChangeType"/>.
        /// </summary>
        private ViewChangeType activeViewChangeType;

        /// <summary>
        /// Stores the position of the mouse of the last time it was captured.
        /// </summary>
        private System.Drawing.Point lastMousePosition;
        
        /// <summary>
        /// Manages the drawing of sprites in the 'view-box' using XNA.
        /// </summary>
        private XnaApplication xnaApplication;

        /// <summary>
        /// The thread the XNA drawing and updating logic runs in.
        /// </summary>
        private Thread xnaThread;

        /// <summary>
        /// Represents the (bindable) application object.
        /// </summary>
        private readonly SpriteToolApplicationViewModel application = new SpriteToolApplicationViewModel( SpriteToolApplication.Current );
    }
}
