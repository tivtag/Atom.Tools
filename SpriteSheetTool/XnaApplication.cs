// <copyright file="XnaApplication.xaml.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteSheetTool.XnaApplication class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteSheetTool
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Atom.Math;
    using Atom.Storage;
    using Atom.Wpf;
    using Atom.Xna;
    using Atom.Xna.Batches;
    using Atom.Xna.Fonts;
    using Atom.Xna.Wpf;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents the Xna Game that is running in the background
    /// of the WPF SpriteSheetTool application.
    /// </summary>
    public sealed class XnaApplication : XnaWpfGame
    {
        /// <summary>
        /// Gets the ITexture2DLoader that can be used to load the Texture2D
        /// assets used by sprites.
        /// </summary>
        public ITexture2DLoader TextureLoader
        {
            get
            {
                return this.textureLoader;
            }
        }

        /// <summary>
        /// Gets the <see cref="ISpriteLoader"/> that provides a mechanism
        /// for loading <see cref="ISprite"/> assets.
        /// </summary>
        public SpriteLoader SpriteLoader
        {
            get
            {
                return this.spriteLoader;
            }
        }

        /// <summary>
        /// The size of the view area.
        /// </summary>
        private static readonly Point2 ViewSize = new Point2( 520, 556 );

        /// <summary>
        /// Initializes a new instance of the XnaApplication class.
        /// </summary>
        /// <param name="controlHandle">
        /// The handle of the control in which Xna is drawing.
        /// </param>
        public XnaApplication( IntPtr controlHandle )
            : base( ViewSize, controlHandle )
        {
            this.Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.spriteLoader = new SpriteLoader();
            this.textureLoader = new UncachedFromFileTexture2DLoader( this.Graphics );
            this.fontLoader = new FontLoader( this.Services );
        }

        /// <summary>
        /// Loads the content used by this XnaApplication.
        /// </summary>
        protected override void LoadContent()
        {
            SafeExecute.WithMsgBox( () => this.DiscoverSpriteDatabases() );

            this.drawContext = new SpriteDrawContext( new ComposedSpriteBatch( this.GraphicsDevice ), this.GraphicsDevice );
            this.font = this.fontLoader.Load( "Tahoma10" );
        }

        /// <summary>
        /// Discovers SpriteDatabases and loads them into memory.
        /// </summary>
        private void DiscoverSpriteDatabases()
        {
            var databaseList = XmlUtilities.TryDeserialize<List<string>>(
                @"Content\Configuration\Sheets\SpriteDatabases.xml",
                ( exception ) => {
                    System.Windows.MessageBox.Show(
                        exception.ToString(),
                        "Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error
                    );

                    return new List<string>();
                }
            );

            foreach( string databasePath in databaseList )
            {
                var database = StorageUtilities.LoadFromFile<SpriteDatabase>(
                    databasePath,
                    new SpriteDatabase.ReaderWriter( this.TextureLoader )
                );

                this.spriteLoader.Insert( database );
            }
        }

        /// <summary>
        /// Shows the given ISpriteSheet in the XNA drawing area.
        /// </summary>
        /// <param name="sheet">
        /// The ISpriteSheet to visualize. Allowed to be null.
        /// </param>
        public void ShowSheet( ISpriteSheet sheet )
        {
            this.sheetDrawComponent.Sheet = sheet;
        }

        /// <summary>
        /// Shows the given ISprite in the XNA drawing area.
        /// </summary>
        /// <param name="sprite">
        /// The ISprite to visualize. Allowed to be null.
        /// </param>
        /// <param name="index">
        /// The zero-based index of the given ISprite into
        /// the SpriteSheet.
        /// </param>
        public void ShowCurrentSprite( ISprite sprite, int index )
        {
            this.sprite = sprite;
            this.spriteIndex = index;
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> in the given location of the current <see cref="ISpriteSheet"/>.
        /// </summary>
        /// <param name="positionX">
        /// The position on the x-axis (in pixels).
        /// </param>
        /// <param name="positionY">
        /// The position on the y-axis (in pixels).
        /// </param>
        /// <param name="index">
        /// Will contain the index of the selected sprite.
        /// </param>
        /// <returns>
        /// The ISprite that was found at the given location.
        /// </returns>
        public ISprite GetSpriteAt( int positionX, int positionY, out int index )
        {
            Vector2 transformedPosition = Vector2.Transform( new Vector2( positionX, positionY ), viewTransform.Inverse );
            return sheetDrawComponent.GetSpriteAt( (Point2)transformedPosition, out index );
        }

        /// <summary>
        /// Zooms the view in or out.
        /// </summary>
        /// <param name="factor">
        /// The zoom factor to apply.
        /// </param>
        public void ZoomView( float factor )
        {
            if( sheetDrawComponent.Sheet == null )
                return;

            this.viewTransform.M11 += factor;
            this.viewTransform.M22 += factor;
        }

        /// <summary>
        /// Moves the view.
        /// </summary>
        /// <param name="offset"></param>
        public void TranslateView( Vector2 offset )
        {
            if( sheetDrawComponent.Sheet == null )
                return;

            this.viewTransform.Translation += new Vector3( (int)offset.X / 2, (int)offset.Y / 2, 0.0f );
        }

        /// <summary>
        /// Resets the view transformation.
        /// </summary>
        public void ResetView()
        {
            this.viewTransform = Matrix4.Identity;
        }

        /// <summary>
        /// Draws the next frame.
        /// </summary>
        /// <param name="gameTime">
        /// The current GameTime.
        /// </param>
        protected override void Draw( Xna.GameTime gameTime )
        {
            this.GraphicsDevice.Clear( Xna.Color.Gray );
            this.drawContext.GameTime = gameTime;

            this.drawContext.Begin( BlendState.NonPremultiplied, SamplerState.PointClamp, SpriteSortMode.Immediate, viewTransform );
            {
                sheetDrawComponent.Draw( this.drawContext );
                DrawSpriteSelection();
            }
            this.drawContext.End();

            this.drawContext.Begin( BlendState.NonPremultiplied, SamplerState.PointClamp, SpriteSortMode.Immediate );
            {
                this.DrawUserInterface();
            }
            this.drawContext.End();
        }

        /// <summary>
        /// Draws the unscaled part of the UI.
        /// </summary>
        private void DrawUserInterface()
        {
            if( this.sprite != null )
            {
                var position = new Vector2(
                    ViewSize.X - 25.0f - this.sprite.Size.X,
                    25.0f
                );

                this.sprite.Draw( position, drawContext.Batch );
                this.font.Draw( this.sprite.Name, new Vector2( ViewSize.X - 25.0f, 50.0f ), TextAlign.Right, Xna.Color.White, this.drawContext );
                this.font.Draw( this.spriteIndex.ToString( CultureInfo.InvariantCulture ), new Vector2( ViewSize.X - 25.0f, 65.0f ), TextAlign.Right, Xna.Color.White, this.drawContext );
            }
        }

        private void DrawSpriteSelection()
        {
            if( this.spriteIndex >= 0 )
            {
                Rectangle area = this.sheetDrawComponent.GetSpriteArea( spriteIndex );

                this.drawContext.Batch.DrawRect(
                    area,
                    new Xna.Color( 255, 0, 0, 125 )
                );
            }
        }

        /// <summary>
        /// Updates the application.
        /// </summary>
        /// <param name="gameTime">
        /// The current GameTime.
        /// </param>
        protected override void Update( Xna.GameTime gameTime )
        {
            this.updateContext.GameTime = gameTime;
            this.sheetDrawComponent.Update( this.updateContext );
        }

        private IFont font;

        /// <summary>
        /// The view transformation matrix used when rendering the sprites.
        /// </summary>
        private Matrix4 viewTransform = Matrix4.Identity;

        /// <summary>
        /// The ISprite that is currently selected.
        /// </summary>
        private ISprite sprite;

        /// <summary>
        /// The index of the sprite that is currently selected.
        /// </summary>
        private int spriteIndex;

        /// <summary> 
        /// The context under which each drawing operation occurs.
        /// </summary>
        private SpriteDrawContext drawContext;

        /// <summary>
        /// The ITexture2DLoader that can be used to load the Texture2D
        /// assets used by sprites.
        /// </summary>
        private readonly ITexture2DLoader textureLoader;

        /// <summary>
        /// The IFontLoader that is used to load the IFont that is used to draw text to the screen.
        /// </summary>
        private readonly IFontLoader fontLoader;

        /// <summary>
        /// Provides a mechanism for loading <see cref="ISprite"/> assets.
        /// </summary>
        private readonly SpriteLoader spriteLoader;

        /// <summary>
        /// The context under which each update operation occurs.
        /// </summary>
        private readonly IXnaUpdateContext updateContext = new XnaUpdateContext();

        /// <summary>
        /// Implements a mechanism that draws ISpriteSheets.
        /// </summary>
        private readonly SpriteSheetDrawComponent sheetDrawComponent = new SpriteSheetDrawComponent();
    }
}
