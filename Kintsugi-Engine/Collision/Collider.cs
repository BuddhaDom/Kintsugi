using Kintsugi.Objects;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kintsugi.Collision.Collider;

namespace Kintsugi.Collision
{
    public class Collider
    {
        public HashSet<string> BelongLayers { get; internal set; } = [];
        public HashSet<string> CollideLayers { get; internal set; } = [];
        public bool IsTrigger { get; internal set; }

        public virtual void OnTriggerCollision(Collider other)
        {
            Console.WriteLine("Trigger collision between " + this + " and " + other);
        }
    }
    public interface TileObjectColliderInitialize
    {
        void Initialize(TileObject t);
    }
    public class TileObjectCollider : Collider, TileObjectColliderInitialize
    {
        public TileObject TileObject { get; private set; }
        public void Initialize(TileObject t)
        {
            TileObject = t;
        }
    }
    public class GridlayerCollider : Collider
    {
        /*
        public GridLayer GridLayer { get; }
        internal GridlayerCollider(GridLayer g)
        {
            GridLayer = g;
        }*/
    }
}
