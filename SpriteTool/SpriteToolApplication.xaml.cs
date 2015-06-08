// <copyright file="SpriteToolApplication.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.SpriteToolApplication class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool
{
    using System;
    using System.Windows;

    /// <summary>
    /// The SpriteTool application allows the creation and manipulation of <see cref="Atom.Xna.ISprite"/>s.
    /// </summary>
    public sealed partial class SpriteToolApplication : Application
    {
        /// <summary>
        /// Gets the currently running SpriteToolApplication instance.
        /// </summary>
        public static new SpriteToolApplication Current
        {
            get
            {
                return (SpriteToolApplication)Application.Current;
            }
        }

        /// <summary>
        /// Initializes a new instance of the SpriteToolApplication class.
        /// </summary>
        public SpriteToolApplication()
        {
            this.InitializeExceptionHandling();
        }

        /// <summary>
        /// Initializes the handling of unhandled exception that
        /// were caused in this SpriteToolApplication.
        /// </summary>
        private void InitializeExceptionHandling()
        {
            AppDomain.CurrentDomain.UnhandledException += this.OnUnhandledException;
        }

        /// <summary>
        /// Called when an unhandled exception has occurred in the current AppDomain.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The UnhandledExceptionEventArgs that contain the event data.
        /// </param>
        private void OnUnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            MessageBox.Show(
                this.MainWindow,
                e.ExceptionObject.ToString(),
                Atom.ErrorStrings.AnUnhandledExceptionOccurred,
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
