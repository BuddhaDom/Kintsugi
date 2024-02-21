using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Collision
{
    public static class CollisionSystem
    {
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

        public static List<TileObjectCollider> GetCollidingTriggers(TileObject tileObject, Vec2Int position)
        {
            
            List<TileObjectCollider> colliders;
            foreach (var item in collidingObject)
            {
                if (collidesWith.Contains(item))
                {
                    return true;
                }
            }
            return false;

        }

        public static bool TileobjectCollidesAt(TileObject tileObject, Vec2Int position)
        {
            return false;
        }
    }
}
