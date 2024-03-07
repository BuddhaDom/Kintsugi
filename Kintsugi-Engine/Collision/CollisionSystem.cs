using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace Kintsugi.Collision
{
    /// <summary>
    /// How a collision outside the grid should be handled.
    /// </summary>
    public enum VoidCollideMode
    {
        /// <summary>
        /// Checking collisions outside the grid will always report as a collision
        /// </summary>
        always,
        /// <summary>
        /// Checking collisions outside the grid will never report as a collision
        /// </summary>
        never,
        /// <summary>
        /// Checking collisions outside the grid will report as a collision if checking against layer "void"
        /// </summary>
        voidlayer
    };

    public static class CollisionSystem
    {
        /// <summary>
        /// Behavior when checking a collision outside the grid.
        /// </summary>
        public static VoidCollideMode VoidCollideMode { get; set; } = VoidCollideMode.always;
        private static bool VoidCollision(Collider c)
        {
            switch (VoidCollideMode)
            {
                case VoidCollideMode.always:
                    return true;
                case VoidCollideMode.never:
                    return false;
                case VoidCollideMode.voidlayer:
                    return CollidesColliderWithCollider(c, Collider.VoidCollider);
                default:
                    return true;
            }
        }

        internal static bool CollisionLayerOverlaps(HashSet<string> collidingObject, HashSet<string> collidesWith)
        {
            foreach (var colliderBelong in collidingObject)
            {
                if (collidesWith.Contains(colliderBelong))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if <paramref name="collider"/> would collide with <paramref name="otherCollider"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Will never report collision if any are triggers.
        /// </summary>
        public static bool CollidesColliderWithCollider(Collider collider, Collider otherCollider, bool triggerCollisionMode = false)
        {
            if (triggerCollisionMode)
            {
                if ((collider.IsTrigger || otherCollider.IsTrigger) && CollisionLayerOverlaps(collider.CollideLayers, otherCollider.BelongLayers))
                {
                    return true;
                }
            }
            else
            {
                if (!collider.IsTrigger && !otherCollider.IsTrigger && CollisionLayerOverlaps(collider.CollideLayers, otherCollider.BelongLayers))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if a <paramref name="collider"/> would collide with anything at <paramref name="position"/> in <paramref name="grid"/>.
        /// Checks against all tileobjects at <paramref name="position"/> on the <paramref name="grid"/> and all gridlayers on the <paramref name="grid"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Will never report collision if any are triggers.
        /// </summary>
        public static bool CollidesColliderWithPosition(Collider collider, Grid grid, Vec2Int position)
        {
            if (CollidesColliderWithTileobjectsAtPosition(collider, grid, position))
            {
                return true;
            }

            if (grid != null && CollidesColliderWithGridAtPosition(collider, grid, position))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all trigger collisions when <paramref name="collider"/> would collide with anything at <paramref name="position"/> in <paramref name="grid"/>.
        /// Checks against all tileobjects at <paramref name="position"/> on the <paramref name="grid"/> and all gridlayers on the <paramref name="grid"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Atleast one of the colliders must be a trigger in each collision. 
        /// </summary>
        public static List<Collider> GetCollisionsColliderWithPosition(Collider collider, Grid grid, Vec2Int position, bool triggerCollisionMode = false)
        {
            List<Collider> colliders = new();
            if (collider == null || grid == null) return colliders;

            colliders.AddRange(GetCollisionsColliderWithTileobjectsAtPosition(collider, grid, position, triggerCollisionMode));

            colliders.AddRange(GetCollisionsColliderWithGridAtPosition(collider, grid, position, triggerCollisionMode));

            return colliders;
        }

        /// <summary>
        /// Check if a <paramref name="collider"/> would collide with any <see cref="TileObject"/> at <paramref name="position"/> in <paramref name="grid"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Will never report collision if any are triggers.
        /// </summary>
        public static bool CollidesColliderWithTileobjectsAtPosition(Collider collider, Grid grid, Vec2Int position)
        {
            if (grid == null) return false;
            var otherObjects = grid.GetObjectsAtPosition(position);
            if (otherObjects == null) return false;

            foreach (var otherObject in otherObjects)
            {
                if (otherObject.Collider != null && CollidesColliderWithCollider(collider, otherObject.Collider))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all trigger collisions when <paramref name="collider"/> would collide with any <see cref="TileObject"/> at <paramref name="position"/> in <paramref name="grid"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Atleast one of the colliders must be a trigger in each collision. 
        /// </summary>
        public static List<Collider> GetCollisionsColliderWithTileobjectsAtPosition(Collider collider, Grid grid, Vec2Int position, bool triggerCollisionMode = false)
        {
            List<Collider> colliders = new();
            if (collider == null) return colliders;

            var otherObjects = grid.GetObjectsAtPosition(position);
            if (otherObjects == null) return colliders;

            foreach (var otherObject in otherObjects)
            {
                if (otherObject.Collider != null && CollidesColliderWithCollider(collider, otherObject.Collider, triggerCollisionMode))
                {
                    colliders.Add(otherObject.Collider);
                }
            }
            return colliders;
        }

        /// <summary>
        /// Check if a <paramref name="collider"/> would collide with any <see cref="GridLayer"/> at <paramref name="position"/> in <paramref name="grid"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Will never report collision if any are triggers.
        /// </summary>
        public static bool CollidesColliderWithGridAtPosition(Collider collider, Grid grid, Vec2Int position)
        {
            if (collider == null) return false;

            int x, y;
            x = position.x;
            y = position.y;

            if (!grid.IsGridPositionWithinGrid(position))
            {
                return VoidCollision(collider);
            }

            foreach (var gridLayer in grid.Layers)
            {
                if (CollidesColliderWithGridLayerAtPosition(collider, gridLayer, position))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all trigger collisions when a <paramref name="collider"/> would collide with any <see cref="GridLayer"/> at <paramref name="position"/> in <paramref name="grid"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Atleast one of the colliders must be a trigger in each collision. 
        /// </summary>
        public static List<Collider> GetCollisionsColliderWithGridAtPosition(Collider collider, Grid grid, Vec2Int position, bool triggerCollisionMode = false)
        {
            List<Collider> colliders = new();

            if (collider == null) return colliders;

            foreach (var gridLayer in grid.Layers)
            {
                colliders.AddRange(GetCollisionsColliderWithGridlayerAtPosition(collider, gridLayer, position, triggerCollisionMode));
            }
            return colliders;
        }

        /// <summary>
        /// Gets all trigger collisions when <paramref name="grid"/> collides against all <see cref="TileObject"/> at <paramref name="position"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Atleast one of the colliders must be a trigger in each collision. 
        /// </summary>
        public static List<Collider> GetCollisionsGridAtPositionWithTileobjectsAtPosition(Grid grid, Vec2Int position, bool triggerCollisionMode = false)
        {
            List<Collider> colliders = new();

            foreach (var gridLayer in grid.Layers)
            {
                colliders.AddRange(GetCollisionsGridlayerAtPositionWithTileobjectsAtPosition(grid, gridLayer, position, triggerCollisionMode));
            }
            return colliders;
        }

        /// <summary>
        /// Check if a <paramref name="collider"/> would collide with <paramref name="gridLayer"/> at <paramref name="position"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Will never report collision if any are triggers.
        /// </summary>
        public static bool CollidesColliderWithGridLayerAtPosition(Collider collider, GridLayer gridLayer, Vec2Int position)
        {
            if (collider == null || gridLayer.Collider == null) return false;

            if (gridLayer.Collider.IsTrigger) return false;

            if (!gridLayer.IsGridPositionWithinGrid(position))
            {
                return VoidCollision(collider);
            }

            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                if (CollisionSystem.CollidesColliderWithCollider(collider, gridLayer.Collider))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets all trigger collisions when a <paramref name="collider"/> would collide with <paramref name="gridLayer"/> at <paramref name="position"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Atleast one of the colliders must be a trigger in each collision. 
        /// </summary>
        public static List<Collider> GetCollisionsColliderWithGridlayerAtPosition(Collider collider, GridLayer gridLayer, Vec2Int position, bool triggerCollisionMode = false)
        {
            List<Collider> colliders = new();
            if (collider == null || gridLayer.Collider == null) return colliders;

            if (!gridLayer.IsGridPositionWithinGrid(position))
            {
                colliders.Add(Collider.VoidCollider);
                return colliders;
            }

            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                if (CollisionSystem.CollidesColliderWithCollider(collider, gridLayer.Collider, triggerCollisionMode))
                {
                    colliders.Add(gridLayer.Collider);
                    Console.WriteLine("Trying to get trigger collider on grid layer");
                }
            }
            return colliders;
        }

        /// <summary>
        /// Gets all trigger collisions when <paramref name="gridLayer"/> collides against all <see cref="TileObject"/> at <paramref name="position"/>.
        /// Is a one way check, does not check the reverse direction.
        /// Atleast one of the colliders must be a trigger in each collision. 
        /// </summary>
        public static List<Collider> GetCollisionsGridlayerAtPositionWithTileobjectsAtPosition(Grid grid, GridLayer gridLayer, Vec2Int position, bool triggerCollisionMode = false)
        {
            List<Collider> colliders = new();
            if (gridLayer.Collider == null) return colliders;

            if (!gridLayer.IsGridPositionWithinGrid(position))
            {
                return colliders;
            }


            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                foreach (var tileObject in grid.GetObjectsAtPosition(position) ?? Enumerable.Empty<TileObject>())
                {
                    if (tileObject.Collider == null) continue;

                    if (CollisionSystem.CollidesColliderWithCollider(gridLayer.Collider, tileObject.Collider, triggerCollisionMode))
                    {
                        colliders.Add(gridLayer.Collider);
                        Console.WriteLine("Trying to get trigger collider on grid layer");
                    }
                }
            }
            return colliders;
        }
    }
}
