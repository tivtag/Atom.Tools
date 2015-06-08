// <copyright file="SpriteAtlasBuilder.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Atom.Tools.SpriteTool.Atlas.SpriteAtlasBuilder class.
// </summary>
// <author>
//     Paul Ennemoser (Tick)
// </author>

namespace Atom.Tools.SpriteTool.Atlas
{
    using System.IO;
    using Atom.Math;
    using Atom.Xna;

    /// <summary>
    /// Implements a mechanism that builds a <see cref="SpriteAtlas"/> from a <see cref="RenderedTextureAtlas"/>-
    /// </summary>
    public sealed class SpriteAtlasBuilder
    {
        /// <summary>
        /// Attempts to build a <see cref="SpriteAtlas"/> from the given <see cref="RenderedTextureAtlas"/>.
        /// </summary>
        /// <param name="renderedTextureAtlas">
        /// The input RenderedTextureAtlas.
        /// </param>
        /// <returns>
        /// The output SpriteAtlas.
        /// </returns>
        public Atom.Xna.SpriteAtlas Build( RenderedTextureAtlas renderedTextureAtlas )
        {
            var atlas = new SpriteAtlas(){
                Texture = renderedTextureAtlas.AtlasTexture
            };

            foreach( TextureAtlasEntry entry in renderedTextureAtlas.Atlas.Entries )
            {
                var originalTexture = entry.Texture;

                Sprite sprite = new Sprite(){
                    Name = Path.GetFileNameWithoutExtension( originalTexture.Name ),
                    Source = new Rectangle( entry.Placement.X, entry.Placement.Y, originalTexture.Width, originalTexture.Height ),
                    Texture = atlas.Texture
                };

                atlas.Sprites.Add( sprite );
            }

            atlas.Verify();
            return atlas;
        }
    }
}