
namespace Atom.Tools.SpriteTool.Atlas.Packing
{
    using System;
    using Atom.Math;

    /// <summary>
    /// Represents the abstract base class for rectangle packing algorithms.
    /// </summary>
    /// <remarks>
    /// An almost exhaustive list of packing algorithms can be found here:
    /// http://www.csc.liv.ac.uk/~epa/surveyhtml.html
    /// </remarks>
    public abstract class BaseRectanglePacker : IRectanglePacker
    {
        /// <summary>
        /// Initializes a new instance of the RectanglePacker class.
        /// </summary>
        /// <param name="packingArea">
        /// The size of the area that can be filled with rectangles.
        /// </param>
        protected BaseRectanglePacker( Point2 packingArea )
        {
            this.packingArea = packingArea;
        }

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
        public abstract bool TryPack( int rectangleWidth, int rectangleHeight, out Point2 placement );

        /// <summary>
        /// Gets the maximum width the packing area is allowed to have.
        /// </summary>
        protected int PackingAreaWidth
        {
            get
            {
                return this.packingArea.X;
            }
        }

        /// <summary>
        /// Gets the maximum height the packing area is allowed to have.
        /// </summary>
        protected int PackingAreaHeight
        {
            get
            { 
                return this.packingArea.Y;
            }
        }

        /// <summary>
        /// The size of the area that can be filled with rectangles.
        /// </summary>
        private readonly Point2 packingArea;
    }
}
