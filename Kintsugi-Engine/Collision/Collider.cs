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
        private static Collider _voidCollider;
        public static Collider VoidCollider
        {
            get {
                if (_voidCollider == null)
                {
                    _voidCollider = new();
                    _voidCollider.BelongLayers.Add("void");
                }
                return _voidCollider;
            }
        }
        /// <summary>
        /// Layers that this collider belongs to, that others can collide with.
        /// </summary>
        public HashSet<string> BelongLayers { get; internal set; } = [];

        /// <summary>
        /// Layers that this collider will collide against.
        /// </summary>
        public HashSet<string> CollideLayers { get; internal set; } = [];

        /// <summary>
        /// Whether collider is a trigger.
        /// Triggers will never cause normal collisions, but will instead cause trigger collisions that activate <see cref="OnTriggerCollision(Collider)">
        /// </summary>
        public bool IsTrigger { get; internal set; }

        /// <summary>
        /// Called when a colliding with a trigger, or when this is a trigger that has collided with something. 
        /// </summary>
        public virtual void OnTriggerCollision(Collider other)
        {
            Console.WriteLine("Trigger collision between " + this + " and " + other);
        }
    }

    public interface TileObjectColliderInitialize
    {
        internal void Initialize(TileObject t);
    }
    public interface GridLayerColliderInitialize
    {
        internal void Initialize(GridLayer t);
    }

    /// <summary>
    /// <see cref="Collider"> on a <see cref="GridLayer">. 
    /// </summary>
    public class GridLayerCollider : Collider, GridLayerColliderInitialize
    {
        public GridLayer GridLayer { get; private set; }
        void GridLayerColliderInitialize.Initialize(GridLayer t)
        {
            GridLayer = t;
        }
    }
}
