using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.Tiles;

namespace Kintsugi.AI
{
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
}
