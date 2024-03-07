using Kintsugi.Core;

namespace Kintsugi.AI
{
    public class PathfindingResult
    {
        internal PathfindingResult(Vec2Int startPosition, Dictionary<Vec2Int, Vec2Int> fromDict, Dictionary<Vec2Int, float> costTo)
        {
            StartPosition = startPosition;
            this.fromDict = fromDict;
            this.costTo = costTo;
        }
        public Vec2Int StartPosition { get; }
        private Dictionary<Vec2Int, Vec2Int> fromDict = new();
        private Dictionary<Vec2Int, float> costTo = new();

        public IEnumerable<Vec2Int> ReachablePositions()
        {
            return costTo.Keys;
        }
        public Vec2Int? ComesFrom(Vec2Int position)
        {
            if (fromDict.TryGetValue(position, out Vec2Int from))
            {
                return from;
            }
            return null;
        }
        public float GetCost(Vec2Int position)
        {
            if (costTo.TryGetValue(position, out float cost))
            {
                return cost;
            }
            return float.PositiveInfinity;
        }
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
