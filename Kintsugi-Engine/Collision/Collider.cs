using Kintsugi.Objects;
using Kintsugi.Tiles;

namespace Kintsugi.Collision
{
    /// <summary>
    /// Represents collision properties applicable to objects and layers.
    /// </summary>
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
        /// Triggers will never cause normal collisions, but will instead cause trigger collisions that activate <see cref="OnTriggerCollision(Collider)"/>
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

    /// <summary>
    /// Makes a collider class require initializing a <see cref="TileObject"/>
    /// </summary>
    public interface TileObjectColliderInitialize
    {
        /// <summary>
        /// Initialize a <see cref="TileObject"/> for this collider class.
        /// </summary>
        /// <param name="t">The <see cref="TileObject"/> reference to use when initializing.</param>
        internal void Initialize(TileObject t);
    }
    /// <summary>
    /// Makes a collider class require initializing a <see cref="GridLayer"/>
    /// </summary>
    public interface GridLayerColliderInitialize
    {
        /// <summary>
        /// Initialize a <see cref="GridLayer"/> for this collider class.
        /// </summary>
        /// <param name="t">The <see cref="GridLayer"/> reference to use when initializing.</param>
        internal void Initialize(GridLayer t);
    }

    /// <summary>
    /// <see cref="Collider"/> on a <see cref="GridLayer"/>. 
    /// </summary>
    public class GridLayerCollider : Collider, GridLayerColliderInitialize
    {
        /// <summary>
        /// <see cref="GridLayer"/> which this collider affects.
        /// </summary>
        public GridLayer GridLayer { get; private set; }
        void GridLayerColliderInitialize.Initialize(GridLayer t)
        {
            GridLayer = t;
        }
    }
}
