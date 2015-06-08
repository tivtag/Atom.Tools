
namespace TileExtractionTool
{
    using Atom.Collections;
    using Microsoft.Xna.Framework;

    internal sealed class ExtractedTile
    {
        public Color[] Data;
        public int X;
        public int Y;

        internal bool MatchesData( ExtractedTile otherTile )
        {
            return this.Data.ElementsEqual( otherTile.Data );
        }
    }
}
