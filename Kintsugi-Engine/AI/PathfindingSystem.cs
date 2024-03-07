using Kintsugi.Core;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.AI
{
    public static class PathfindingSystem
    {
        public static PathfindingResult Dijkstra(Grid grid, Vec2Int startPosition, float maxCost, PathfindingSettings pathfindingSettings = null)
        {
            Dictionary<Vec2Int, Vec2Int> fromDict = new();
            Dictionary<Vec2Int, float> costDict = new();
            PriorityQueue<Vec2Int, float> frontier = new();
            costDict.Add(startPosition, 0);
            frontier.Enqueue(startPosition, costDict[startPosition]);

            while (frontier.Count > 0)
            {
                var currentPos = frontier.Dequeue();
                VisitNeighbor(currentPos + Vec2Int.Left);
                VisitNeighbor(currentPos + Vec2Int.Right);
                VisitNeighbor(currentPos + Vec2Int.Up);
                VisitNeighbor(currentPos + Vec2Int.Down);

                void VisitNeighbor(Vec2Int newPos)
                {
                    var explored = costDict.TryGetValue(newPos, out float oldCost);
                    var newCost = costDict[currentPos] +
                        (pathfindingSettings != null ?
                        pathfindingSettings.GetCost(newPos, grid)
                        : 1);
                    if (newCost > maxCost)
                    {
                        return;
                    }
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
