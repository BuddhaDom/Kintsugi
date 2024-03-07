using Kintsugi.Collision;
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
        private Dictionary<string, float> costDict = new();
        private Dictionary<string, int> priorities = new();
        private float defaultCost = 1;
        private Collider collider = new();
        public bool CheckAgainstTriggers { get; set; }
        public void SetDefaultCost(float cost)
        {
            if (cost < 0)
            {
                throw new Exception("Pathfinding cost cannot be less than zero");
            }
            defaultCost = cost;
        }
        public void AddCollideLayers(HashSet<string> collideLayers, int priority = 0)
        {
            foreach (var item in collideLayers)
            {
                costDict[item] = float.PositiveInfinity;
                priorities[item] = priority;
                collider.CollideLayers.Add(item);
            }
        }
        public void SetCostLayer(string layer, float cost, int priority = 0)
        {
            if (cost < 0)
            {
                throw new Exception("Pathfinding cost cannot be less than zero");
            }
            costDict[layer] = cost;
            priorities[layer] = priority;
            collider.CollideLayers.Add(layer);
        }

        public float GetCost(Vec2Int position, Grid grid)
        {
            var collisions = CollisionSystem.GetCollisionsColliderWithPosition(collider, grid, position);
            if (CheckAgainstTriggers)
            {
                collisions.AddRange(CollisionSystem.GetCollisionsColliderWithPosition(collider, grid, position, true));

            }

            int curPriority = int.MinValue;
            float cost = defaultCost;
            foreach (var item in collisions)
            {
                foreach (var layer in item.BelongLayers)
                {
                    if (layer == "void")
                    {
                        return GetVoidCost();
                    }
                    var priority = priorities[layer];
                    if (priority > curPriority && costDict.TryGetValue(layer, out float newCost))
                    {
                        curPriority = priorities[layer];
                        cost = newCost;
                    }
                }
            }
            return cost;
        }
        private float GetVoidCost()
        {
            switch (CollisionSystem.VoidCollideMode)
            {
                case VoidCollideMode.always: return float.PositiveInfinity;
                case VoidCollideMode.never: return defaultCost;
                case VoidCollideMode.voidlayer:
                    if (costDict.TryGetValue("void", out float cost))
                    {
                        return cost;
                    }
                    return defaultCost;
                default: return defaultCost;
            }

        }
    }
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
