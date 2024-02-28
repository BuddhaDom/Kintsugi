using System.Numerics;
using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using SixLabors.ImageSharp;

namespace Kintsugi.Objects
{
    /// <summary>
    /// An object that can be used and placed into a <see cref="Grid"/>. 
    /// </summary>
    public class TileObject
    {
        /// <summary>
        /// Transform properties of this object.
        /// </summary>
        public TileObjectTransform Transform { get; private set; } = new();
        /// <summary>
        /// Collision properties of this object.
        /// </summary>
        public Collider? Collider { get; private set; }
        /// <summary>
        /// Graphic properties of this object.
        /// </summary>
        public TileObjectSprite? Sprite { get; private set; }

        /// <summary>
        /// Establish the position of this object in a grid system. This method also updates 
        /// </summary>
        /// <param name="position">New coordinates of the object.</param>
        public void SetPosition(Vec2Int position)
        {
            if (Transform.Grid != null)
                RemoveFromGridTileObjects(Transform.Grid);
            Transform.Position = position;
            if (Transform.Grid != null)
                AddToGridTileObjects(Transform.Grid);
            ResolveTriggerCollisions(position);
        }

        private void ResolveTriggerCollisions(Vec2Int pos)
        {
            if (Transform.Grid != null && Collider != null)
            {
                List<Collider> selfTriggers = new();
                List<Collider> otherTriggers = new();

                otherTriggers.AddRange(CollisionSystem.GetCollidingTriggersColliderWithPosition(Collider, Transform.Grid, Transform.Position));
                var otherObjects = Transform.Grid.GetObjectsAtPosition(pos);
                if (otherObjects != null)
                {
                    foreach (var tileObject in otherObjects)
                    {
                        if (tileObject != this && tileObject.Collider != null)
                        {
                            if (CollisionSystem.TriggerCollidesColliderWithCollider(tileObject.Collider, Collider))
                            {
                                selfTriggers.Add(tileObject.Collider);
                            }
                        }
                    }
                    selfTriggers.AddRange(CollisionSystem.GetCollidingTriggersGridAtPositionWithTileobjectsAtPosition(Transform.Grid, Transform.Position));
                }
                
                foreach (var selfTrigger in selfTriggers)
                {
                    selfTrigger.OnTriggerCollision(Collider);
                }
                foreach (var otherTrigger in otherTriggers)
                {
                    Collider.OnTriggerCollision(otherTrigger);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        public void Move(Vec2Int vector)
            => SetPosition(Transform.Position + vector);

        public void RemoveFromGrid()
        {
            if (Transform.Grid == null) return;
            RemoveFromGridTileObjects(Transform.Grid);
            Transform.Grid = null;
        }

        private void RemoveFromGridTileObjects(Grid grid)
        {
            if (!grid.TileObjects.TryGetValue(Transform.Position, out _)) return;
            grid.TileObjects[Transform.Position].Remove(this);
            if (grid.TileObjects[Transform.Position].Count == 0)
                grid.TileObjects.Remove(Transform.Position);
        }

        public void AddToGrid(Grid grid, int layer = 0)
        {
            ArgumentNullException.ThrowIfNull(grid);

            AddToGridTileObjects(grid);
            Transform.Grid = grid;
            Transform.Layer = layer;
        }

        private void AddToGridTileObjects(Grid grid)
        {
            if (grid.TileObjects.TryGetValue(Transform.Position, out var value))
                value.Add(this);
            else
                grid.TileObjects.Add(Transform.Position, new List<TileObject> { this });
        }

        public void SetCollider(HashSet<string> belongLayers, HashSet<string> collideLayers, bool isTrigger = false)
        {
            Collider ??= new TileObjectCollider();
            ((TileObjectColliderInitialize)Collider).Initialize(this);
            Collider.IsTrigger = isTrigger;
            Collider.BelongLayers = belongLayers;
            Collider.CollideLayers = collideLayers;
        }
        public void SetColliderTyped<T>(HashSet<string> belongLayers, HashSet<string> collideLayers, bool isTrigger = false)
            where T: TileObjectCollider, TileObjectColliderInitialize, new()
        {
            Collider = new T();
            ((TileObjectColliderInitialize)Collider).Initialize(this);
            Collider.IsTrigger = isTrigger;
            Collider.BelongLayers = belongLayers;
            Collider.CollideLayers = collideLayers;
        }

        public void SetSprite(string path, Vector2 tilePivot = default, Vector2 imagePivot = default)
        {
            // Initialize the property.
            Sprite ??= new TileObjectSprite();
            Sprite.Path = path;
            Sprite.TilePivot = tilePivot;
            Sprite.ImagePivot = imagePivot;

            // Get the Height and Width
            if (path == "") return;
            var image = Image.Load(path);
            Sprite.Height = image.Height;
            Sprite.Width = image.Width;
            image.Dispose();
        }
    }

    namespace Properties
    {
        public class TileObjectTransform
        {
            public Vec2Int Position { get; internal set; } = Vec2Int.Zero;
            public Facing Facing { get; internal set; } = Facing.East;
            public Grid? Grid { get; internal set; }
            public int Layer { get; internal set; }
        }

        public class TileObjectSprite
        {
            public string Path { get; internal set; } = "";
            /// <summary>
            /// Position on the tile from which the object is rendered.
            /// Defined between <see cref="Vector2.Zero"/> and <see cref="Vector2.One"/> as the upper and lower bounds of the tile width.
            /// </summary>
            public Vector2 TilePivot { get; internal set; } = Vector2.Zero;
            /// <summary>
            /// Position on the sprite which will match positions with the <see cref="TilePivot"/>.
            /// Defined between this sprite's <see cref="Height"/> and <see cref="Width"/>. 
            /// </summary>
            public Vector2 ImagePivot { get; internal set; } = Vector2.Zero;

            public int Height { get; internal set; }

            public int Width { get; internal set; }
        }
    }

}
