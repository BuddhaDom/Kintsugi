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
        public static bool CollideWith(TileObjectCollider collider, TileObjectCollider otherCollider)
        {
            if (!collider.IsTrigger && !otherCollider.IsTrigger && CollisionLayerOverlaps(collider.CollideLayers, otherCollider.BelongLayers))
            {
                return true;
            }
            return false;
        }
        public static bool TriggerCollision(TileObjectCollider collider, TileObjectCollider otherCollider)
        {
            if (!collider.IsTrigger && otherCollider.IsTrigger && CollisionLayerOverlaps(collider.CollideLayers, otherCollider.BelongLayers))
            {
                return true;
            }
            return false;
        }
        public static bool Collides(TileObjectCollider collider, Grid grid, Vec2Int position)
        {
            if (TileObjectCollidesWithTileobjects(collider, grid, position))
            {
                return true;
            }

            if (grid != null && TileObjectCollidesWithGrid(collider, grid, position))
            {
                return true;
            }

            return false;
        }
        public static List<TileObjectCollider> GetCollidingTriggers(TileObjectCollider collider, Grid grid, Vec2Int position)
        {
            List<TileObjectCollider> colliders = new();
            if (collider == null || grid == null) return colliders;

            colliders.AddRange(GetCollidingTriggersWithTileobjects(collider, grid, position));

            colliders.AddRange(GetCollidingTriggersWithGrid(collider, grid, position));

            return colliders;
        }
        public static bool TileObjectCollidesWithTileobjects(TileObjectCollider collider, Grid grid, Vec2Int position)
        {
            var otherObjects = grid.GetObjectsAtPosition(position);
            if (otherObjects == null) return false;

            foreach (var otherObject in otherObjects)
            {
                if (otherObject.Collider != null && CollideWith(collider, otherObject.Collider))
                {
                    return true;
                }
            }
            return false;
        }
        public static List<TileObjectCollider> GetCollidingTriggersWithTileobjects(TileObjectCollider collider, Grid grid, Vec2Int position)
        {
            List<TileObjectCollider> colliders = new();
            if (collider == null) return colliders;

            var otherObjects = grid.GetObjectsAtPosition(position);
            if (otherObjects == null) return colliders;

            foreach (var otherObject in otherObjects)
            {
                if (otherObject.Collider != null && TriggerCollision(collider, otherObject.Collider))
                {
                    colliders.Add(otherObject.Collider);
                }
            }
            return colliders;
        }
        public static bool TileObjectCollidesWithGrid(TileObjectCollider collider, Grid grid, Vec2Int position)
        {
            if (collider == null) return false;

            int x, y;
            x = position.x;
            y = position.y;

            foreach (var gridLayer in grid.Layers)
            {
                if (TileObjectCollidesWithGridLayer(collider, gridLayer, position))
                {
                    return true;
                }
            }
            return false;
        }
        public static List<TileObjectCollider> GetCollidingTriggersWithGrid(TileObjectCollider collider, Grid grid, Vec2Int position)
        {
            List<TileObjectCollider> colliders = new();

            if (collider == null) return colliders;

            foreach (var gridLayer in grid.Layers)
            {
                colliders.AddRange(GetCollidingTriggersWithGridlayer(collider, gridLayer, position));
            }
            return colliders;
        }
        public static bool TileObjectCollidesWithGridLayer(TileObjectCollider collider, GridLayer gridLayer, Vec2Int position)
        {
            if (collider == null) return false;

            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                foreach (var gridCollisionLayer in gridLayer.CollisionLayers)
                {
                    if (collider.CollideLayers.Contains(gridCollisionLayer))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static List<TileObjectCollider> GetCollidingTriggersWithGridlayer(TileObjectCollider collider, GridLayer gridLayer, Vec2Int position)
        {
            List<TileObjectCollider> colliders = new();
            if (collider == null) return colliders;

            if (!gridLayer.Tiles[position.x, position.y].IsEmpty)
            {
                foreach (var gridCollisionLayer in gridLayer.CollisionLayers)
                {
                    if (collider.CollideLayers.Contains(gridCollisionLayer))
                    {
                        throw new NotImplementedException("No colliders on grid layers yet");
                        //return colliders.Add();
                    }
                }
            }
            return colliders;
        }
    }
}
