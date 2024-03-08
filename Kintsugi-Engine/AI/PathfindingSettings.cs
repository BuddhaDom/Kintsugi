using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.Tiles;

namespace Kintsugi.AI
{
    /// <summary>
    /// Settings for the pathfinding algorithms.
    /// Determines which collision layers to collide with, and costs associated with them.
    /// </summary>
    public class PathfindingSettings
    {
        private Dictionary<string, float> costDict = new();
        private Dictionary<string, int> priorities = new();
        private float defaultCost = 1;
        private Collider collider = new();
        /// <summary>
        /// Enable or disable calculating costs against trigger colliders.
        /// </summary>
        public bool CheckAgainstTriggers { get; set; }
        /// <summary>
        /// Sets the default cost of traversal, when no collision layer with assigned cost is collided with.
        /// </summary>
        /// <exception cref="Exception">if cost is less than zero.</exception>
        public void SetDefaultCost(float cost)
        {
            if (cost < 0)
            {
                throw new Exception("Pathfinding cost cannot be less than zero");
            }
            defaultCost = cost;
        }
        /// <summary>
        /// Add collide layers as unwalkable.
        /// </summary>
        /// <param name="collideLayers">Layers that are deemed unwalkable</param>
        /// <param name="priority">Determines which cost is higher priority, high priority rules override low priority rules.</param>
        public void AddCollideLayers(HashSet<string> collideLayers, int priority = 0)
        {
            foreach (var item in collideLayers)
            {
                costDict[item] = float.PositiveInfinity;
                priorities[item] = priority;
                collider.CollideLayers.Add(item);
            }
        }
        /// <summary>
        /// Add a cost rule to a specific collide layer.
        /// </summary>
        /// <param name="layer">Collide layer that will be assigned a cost.</param>
        /// <param name="cost">The cost of traversing this collide layer.</param>
        /// <param name="priority">Determines which cost is higher priority, high priority rules override low priority rules.</param>
        /// <exception cref="Exception">if cost is less than zero.</exception>
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

        /// <summary>
        /// Gets the cost of traversing a specific position on a grid with these settings.
        /// </summary>
        /// <param name="position">Queried position.</param>
        /// <param name="grid">Queried grid.</param>
        /// <returns>Cost traversing the position on the grid, with these settings.</returns>
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
