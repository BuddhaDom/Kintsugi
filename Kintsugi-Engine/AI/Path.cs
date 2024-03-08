using Kintsugi.Core;

namespace Kintsugi.AI
{
    /// <summary>
    /// Represents a path of positions from start to end.
    /// </summary>
    public class Path
    {
        internal Path(List<Vec2Int> pathPositions, float cost)
        {
            PathPositions = pathPositions;
            Cost = cost;
        }
        /// <summary>
        /// Total cost of path.
        /// </summary>
        public float Cost { get; }
        /// <summary>
        /// Positions in path, including start and end positions.
        /// </summary>
        public List<Vec2Int> PathPositions { get; }
    }
}
