using Kintsugi.Core;

namespace Kintsugi.AI
{
    /// <summary>
    /// The result of a pathfinding algorithm from a start position.
    /// Can be queried for reachable positions, their costs, and the paths to those positions.
    /// </summary>
    public class PathfindingResult
    {
        internal PathfindingResult(Vec2Int startPosition, Dictionary<Vec2Int, Vec2Int> fromDict, Dictionary<Vec2Int, float> costTo)
        {
            StartPosition = startPosition;
            this.fromDict = fromDict;
            this.costTo = costTo;
        }
        /// <summary>
        /// The start position that this pathfinding result is based on.
        /// </summary>
        public Vec2Int StartPosition { get; }
        private Dictionary<Vec2Int, Vec2Int> fromDict = new();
        private Dictionary<Vec2Int, float> costTo = new();

        /// <summary>
        /// Enumerable of all reachable positions.
        /// </summary>
        /// <returns>Enumerable of all reachable positions. Includes start position.</returns>
        public IEnumerable<Vec2Int> ReachablePositions()
        {
            return costTo.Keys;
        }
        /// <summary>
        /// Gets the position that should be taken before reaching <param name="position"/> in all paths.
        /// </summary>
        /// <param name="position">Position queried.</param>
        /// <returns>The position that is before <paramref name="position"/> in any path</returns>
        public Vec2Int? ComesFrom(Vec2Int position)
        {
            if (fromDict.TryGetValue(position, out Vec2Int from))
            {
                return from;
            }
            return null;
        }

        /// <summary>
        /// Gets the total cost of reaching a specific position.
        /// </summary>
        /// <param name="position">Queried end position</param>
        /// <returns>Cost of path to <paramref name="position"/>, positive infinity if unreachable.</returns>
        public float GetCost(Vec2Int position)
        {
            if (costTo.TryGetValue(position, out float cost))
            {
                return cost;
            }
            return float.PositiveInfinity;
        }
        /// <summary>
        /// Gets the full path to the queried position, if any.
        /// </summary>
        /// <param name="position">Queried end position</param>
        /// <returns>Path from start position to <paramref name="position"/>, including start and end position. Null if none exists.</returns>
        public Path PathTo(Vec2Int position)
        {
            if (position != StartPosition && !fromDict.ContainsKey(position))
            {
                // No path exists
                return null;
            }

            // If this is reached, we are guaranteed a path exists.
            List<Vec2Int> pathPositions = new();
            Vec2Int curPosition = position;
            while (curPosition != StartPosition)
            {
                pathPositions.Add(curPosition);
                curPosition = fromDict[curPosition];
            }
            pathPositions.Add(StartPosition);
            pathPositions.Reverse();
            return new Path(pathPositions, costTo[position]);
        }

    }
}
