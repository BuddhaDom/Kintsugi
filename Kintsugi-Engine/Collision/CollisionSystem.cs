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

namespace Kintsugi.Collision
{
    public static class CollisionSystem
    {
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

        /*
        public static List<TileObjectCollider> GetCollidingTriggers(TileObject tileObject, Vec2Int position)
        {
            
            List<TileObjectCollider> colliders = new();

            var grid = tileObject.Transform.Grid;
            if (grid == null)
            {
                return colliders;
            }
            var otherObjects = grid.GetObjectsAtPosition(position);
            foreach (var otherTileObject in otherObjects)
            {
                if (TileobjectCollidesWithTileobject)
                {
                    return true;
                }
            }
            return false;

        }
        */
        public static bool TileobjectCollidesWithTileobject(TileObject tileObject, TileObject otherTileObject)
        {
            if (tileObject.Collider == null || otherTileObject.Collider == null)
            {
                return false;
            }
            return CollisionLayerOverlaps(tileObject.Collider.CollideLayers, tileObject.Collider.BelongLayers);
        }

        public static bool TileobjectCollidesAt(TileObject tileObject, Vec2Int position)
        {
            var grid = tileObject.Transform.Grid;
            if (grid == null || tileObject.Collider == null) return false;
            
            var otherObjects = grid.GetObjectsAtPosition(position);
            if (otherObjects == null) return false;

            foreach (var otherObject in otherObjects)
            {
                if (TileobjectCollidesWithTileobject(tileObject, otherObject))
                {
                    return true;
                }
            }
            if (tileObject.Transform.Grid != null && TileObjectCollidesWithGrid(tileObject, tileObject.Transform.Grid))
            {
                return true;
            }

            return false;
        }

        public static bool TileObjectCollidesWithGrid(TileObject tileObject, Grid grid)
        {
            if (tileObject.Collider == null) return false;

            int x, y;
            x = tileObject.Transform.Position.x;
            y = tileObject.Transform.Position.y;

            foreach (var gridLayer in grid.Layers)
            {
                if (TileObjectCollidesWithGridLayer(tileObject, gridLayer))
                {
                    return true;
                }
            }
            return false;
        }

        /*
        public static List<CollidingObject> TileObjectGetGridLayerTriggerCollisions(TileObject tileObject, GridLayer gridLayer)
        {
            List<CollidingObject> collisions = new List<CollidingObject>();
            if (tileObject.Collider == null) return collisions;

            int x, y;
            x = tileObject.Transform.Position.x;
            y = tileObject.Transform.Position.y;

            if (!gridLayer.Tiles[x, y].IsEmpty)
            {
                foreach (var gridCollisionLayer in gridLayer.CollisionLayers)
                {
                    if (tileObject.Collider.CollideLayers.Contains(gridCollisionLayer))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        */
        public static bool TileObjectCollidesWithGridLayer(TileObject tileObject, GridLayer gridLayer)
        {
            if (tileObject.Collider == null) return false;

            int x, y;
            x = tileObject.Transform.Position.x;
            y = tileObject.Transform.Position.y;

            if (!gridLayer.Tiles[x, y].IsEmpty)
            {
                foreach (var gridCollisionLayer in gridLayer.CollisionLayers)
                {
                    if (tileObject.Collider.CollideLayers.Contains(gridCollisionLayer))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
