using Kintsugi.Core;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.AI
{
    public class Path
    {
        internal Path(List<Vec2Int> pathPositions, float cost)
        {
            PathPositions = pathPositions;
            Cost = cost;
        }
        public float Cost { get; }
        public List<Vec2Int> PathPositions { get; }
    }
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
    public class PathfindingSettings
    {
    }
    public static class PathfindingSystem
    {
        public static PathfindingResult Dijkstra(Grid grid, Vec2Int startPosition, float maxCost, PathfindingSettings pathfindingResult = null)
        {
            Dictionary<Vec2Int, Vec2Int> fromDict = new();
            Dictionary<Vec2Int, float> costDict = new();
            PriorityQueue<Vec2Int, float> frontier = new();
            costDict.Add(startPosition, 0);
            frontier.Enqueue(startPosition, costDict[startPosition]);

            while (frontier.Count > 0)
            {
                var currentPos = frontier.Dequeue();
                var newCost = costDict[currentPos] + 1;
                if (newCost > maxCost)
                {
                    break;
                }
                VisitNeighbor(currentPos + Vec2Int.Left);
                VisitNeighbor(currentPos + Vec2Int.Right);
                VisitNeighbor(currentPos + Vec2Int.Up);
                VisitNeighbor(currentPos + Vec2Int.Down);

                void VisitNeighbor(Vec2Int newPos)
                {
                    var explored = costDict.TryGetValue(newPos, out float oldCost);
                    if (!explored || newCost < oldCost)
                    {
                        frontier.Enqueue(newPos, newCost);
                        fromDict[newPos] = currentPos;
                        costDict[newPos] = newCost;
                    }
                }
            }

            return new PathfindingResult(startPosition, fromDict, costDict);
        }
    }
}
