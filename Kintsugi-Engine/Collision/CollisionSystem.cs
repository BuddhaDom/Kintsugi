using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Objects.Properties;
using Kintsugi.Physics;
using Kintsugi.Physics.Colliders;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledCS;

namespace Kintsugi.Collision
{
    public static class CollisionSystem
    {
        // current rules:
        // Collisions are one way (a collides with b doesnt imply b collides with a)
        // colliderA vs colliderB checks if A collides with B but not if B collides with A
        // collider vs trigger triggers the trigger
        // trigger vs collider is nothing
        //Check between colliders       !
        //Check between tileobjects     !
        //Collide at                    !
        //Collide at with grid          !
        //Collide at with gridlayer     !
        //collide at with tileobjects   !
        //trigger at                    !
        //trigger at with grid          !
        //trigger at with grid layer    !
        //triggers at with tileobjects  !

        private enum VoidCollideMode { always, never, voidlayer };
        private static VoidCollideMode voidCollideMode = CollisionSystem.VoidCollideMode.never;
        private static Collider voidCollider;
        private static bool VoidCollision(Collider c)
        {
            switch (voidCollideMode)
            {
                case VoidCollideMode.always:
                    return true;
                case VoidCollideMode.never:
                    return false;
                case VoidCollideMode.voidlayer:
                    if (voidCollider == null)
                    {
                        voidCollider = new Collider();
                        voidCollider.BelongLayers.Add("void");
                    }
                    return CollidesColliderWithCollider(c, voidCollider);
                default:
                    return true;
            }
        }

        public static string CollisionLayerEmpty => "";
        internal static bool CollisionLayerOverlaps(HashSet<string> collidingObject, HashSet<string> collidesWith)
        {
            foreach (var item in collidingObject)
            {
                if (collidesWith.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool CollidesColliderWithCollider(Collider collider, Collider otherCollider)
        {
            if (!collider.IsTrigger && !otherCollider.IsTrigger && CollisionLayerOverlaps(collider.CollideLayers, otherCollider.BelongLayers))
            {
                return true;
            }
            return false;
        }
        public static bool TriggerCollidesColliderWithCollider(Collider collider, Collider otherCollider)
        {
            if ((collider.IsTrigger || otherCollider.IsTrigger) && CollisionLayerOverlaps(collider.CollideLayers, otherCollider.BelongLayers))
            {
                return true;
            }
            return false;
        }
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
        public static List<Collider> GetCollidingTriggersColliderWithPosition(Collider collider, Grid grid, Vec2Int position)
        {
            List<Collider> colliders = new();
            if (collider == null || grid == null) return colliders;

            colliders.AddRange(GetCollidingTriggersColliderWithTileobjectsAtPosition(collider, grid, position));

            colliders.AddRange(GetCollidingTriggersColliderWithGridAtPosition(collider, grid, position));

            return colliders;
        }
        public static bool CollidesColliderWithTileobjectsAtPosition(Collider collider, Grid grid, Vec2Int position)
        {
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
        public static List<Collider> GetCollidingTriggersColliderWithTileobjectsAtPosition(Collider collider, Grid grid, Vec2Int position)
        {
            List<Collider> colliders = new();
            if (collider == null) return colliders;

            var otherObjects = grid.GetObjectsAtPosition(position);
            if (otherObjects == null) return colliders;

            foreach (var otherObject in otherObjects)
            {
                if (otherObject.Collider != null && TriggerCollidesColliderWithCollider(collider, otherObject.Collider))
                {
                    colliders.Add(otherObject.Collider);
                }
            }
            return colliders;
        }
        public static bool CollidesColliderWithGridAtPosition(Collider collider, Grid grid, Vec2Int position)
        {
            if (collider == null) return false;

            int x, y;
            x = position.x;
            y = position.y;

            foreach (var gridLayer in grid.Layers)
            {
                if (CollidesColliderWithGridLayerAtPosition(collider, gridLayer, position))
                {
                    return true;
                }
            }
            return false;
        }
        public static List<Collider> GetCollidingTriggersColliderWithGridAtPosition(Collider collider, Grid grid, Vec2Int position)
        {
            List<Collider> colliders = new();

            if (collider == null) return colliders;

            foreach (var gridLayer in grid.Layers)
            {
                colliders.AddRange(GetCollidingTriggersColliderWithGridlayerAtPosition(collider, gridLayer, position));
            }
            return colliders;
        }

        public static List<Collider> GetCollidingTriggersGridAtPositionWithTileobjectsAtPosition(Grid grid, Vec2Int position)
        {
            List<Collider> colliders = new();

            foreach (var gridLayer in grid.Layers)
            {
                colliders.AddRange(GetCollidingTriggersGridlayerAtPositionWithTileobjectsAtPosition(grid, gridLayer, position));
            }
            return colliders;
        }

        public static bool CollidesColliderWithGridLayerAtPosition(Collider collider, GridLayer gridLayer, Vec2Int position)
        {
            if (collider == null || gridLayer.Collider == null) return false;

            if (gridLayer.Collider.IsTrigger) return false;

            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                if (CollisionSystem.CollidesColliderWithCollider(collider, gridLayer.Collider))
                {
                    return true;
                }
            }
            return false;
        }
        public static List<Collider> GetCollidingTriggersColliderWithGridlayerAtPosition(Collider collider, GridLayer gridLayer, Vec2Int position)
        {
            List<Collider> colliders = new();
            if (collider == null || gridLayer.Collider == null) return colliders;

            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                if (CollisionSystem.TriggerCollidesColliderWithCollider(collider, gridLayer.Collider))
                {
                    colliders.Add(gridLayer.Collider);
                    Console.WriteLine("Trying to get trigger collider on grid layer");
                }
            }
            return colliders;
        }
        public static List<Collider> GetCollidingTriggersGridlayerAtPositionWithTileobjectsAtPosition(Grid grid, GridLayer gridLayer, Vec2Int position)
        {
            List<Collider> colliders = new();
            if (gridLayer.Collider == null) return colliders;

            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                foreach (var tileObject in grid.GetObjectsAtPosition(position) ?? Enumerable.Empty<TileObject>())
                {
                    if (tileObject.Collider == null) continue;
                    
                    if (CollisionSystem.TriggerCollidesColliderWithCollider(gridLayer.Collider, tileObject.Collider))
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
