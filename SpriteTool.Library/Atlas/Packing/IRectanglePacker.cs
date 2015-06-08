
namespace Atom.Tools.SpriteTool.Atlas.Packing
{
    using Atom.Math;

    /// <summary>
    /// Provides a mechanism that packages smaller rectangles into a larger rectangle.
    /// </summary>
    public interface IRectanglePacker
    {
        /// <summary>
        /// Tries to allocate space for a rectangle in the packing area</summary>
        /// <param name="rectangleWidth">
        /// The width of the rectangle to allocate.
        /// </param>
        /// <param name="rectangleHeight">
        /// The height of the rectangle to allocate.
        /// </param>
        /// <param name="placement">
        /// Will contain the location at which the rectangle has been placed.
        /// </param>
        /// <returns>
        /// Returns True if space for the rectangle could be allocated;
        /// otherwise false.
        /// </returns>
        bool TryPack( int rectangleWidth, int rectangleHeight, out Point2 placement );
    }
}
