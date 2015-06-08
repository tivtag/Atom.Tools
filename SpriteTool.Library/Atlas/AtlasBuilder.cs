
namespace Atom.Tools.SpriteTool.Atlas
{
    using System.Collections.Generic;
    using System.Linq;
    using Atom.Math;
    using Atom.Tools.SpriteTool.Atlas.Packing;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements a mechanism that builds a <see cref="TextureAtlas"/>.
    /// </summary>
    public sealed class AtlasBuilder
    {
        /// <summary>
        /// Builds a <see cref="TextureAtlas"/> of specific size by trying to insert all of the
        /// specified images with a minimum specified spacing around them.
        /// </summary>
        /// <param name="atlasSize">
        /// The size of the atlas.
        /// </param>
        /// <param name="spacing">
        /// The minimum spacing between the individual images in the Atlas.
        /// </param>
        /// <param name="images">
        /// The images to insert into the atlas.
        /// </param>
        /// <param name="atlas">
        /// The TextureAtlas that has been created.
        /// </param>
        /// <returns>
        /// True if all images have been inserted into the TextureAtlas;
        /// otherwise false if available space ran out.
        /// </returns>
        public bool TryBuild( Point2 atlasSize, int spacing, IEnumerable<Texture2D> images, out TextureAtlas atlas )
        {
            atlas = new TextureAtlas( atlasSize, images.Count() );
            IRectanglePacker packer = new Packing.ArevaloRectanglePacker( atlasSize, images.Count() );

            bool allImagesWereInserted = true;
            int doubleSpacing = 2 * spacing;

            foreach( var image in images )
            {
                int width = image.Width + doubleSpacing;
                int height = image.Height + doubleSpacing;

                Point2 placement;
                if( packer.TryPack( width, height, out placement ) )
                {
                    placement += spacing;
                    atlas.AddEntry( new TextureAtlasEntry( image, placement ) );
                }
                else
                {
                    allImagesWereInserted = false;
                }
            }

            return allImagesWereInserted;
        }
    }
}
