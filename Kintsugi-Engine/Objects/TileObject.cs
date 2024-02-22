using System.Numerics;
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
        public TileObjectCollider? Collider { get; private set; }
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
            if(Transform.Grid != null)
                RemoveFromGridTileObjects(Transform.Grid);
            Transform.Position = position;
            if(Transform.Grid != null)
                AddToGridTileObjects(Transform.Grid);
        }

        /// <summary>
        /// Move this object towards a target vector.
        /// </summary>
        /// <param name="vector">The direction to move to.</param>
        public void Move(Vec2Int vector)
            => SetPosition(Transform.Position + vector);

        /// <summary>
        /// Remove this objects grid. Does nothing if the grid is already <c>null</c>.
        /// </summary>
        public void RemoveFromGrid()
        {
            if (Transform.Grid == null) return;
            RemoveFromGridTileObjects(Transform.Grid);
            Transform.Grid = null;
        }

        /// <summary>
        /// Remove this object from the target grid's <see cref="Grid.TileObjects"/> property.
        /// </summary>
        /// <param name="grid">Target grid to affect.</param>
        private void RemoveFromGridTileObjects(Grid grid)
        {
            ArgumentNullException.ThrowIfNull(grid);
            
            if (grid.TileObjects.TryGetValue(Transform.Position, out _)) return;
                grid.TileObjects[Transform.Position].Remove(this);
                
            if (grid.TileObjects[Transform.Position].Count == 0)
                grid.TileObjects.Remove(Transform.Position);
        }

        /// <summary>
        /// Add this tile object to a grid, and on a specific layer.
        /// </summary>
        /// <param name="grid">Target grid to place the object in.</param>
        /// <param name="layer">Layer of the grid to which this tile object will belong to.</param>
        public void AddToGrid(Grid grid, int layer)
        {
            ArgumentNullException.ThrowIfNull(grid);
            AddToGridTileObjects(grid);
            Transform.Grid = grid;
            Transform.Layer = layer;
        }

        /// <summary>
        /// Add this object to a target grid's <see cref="Grid.TileObjects"/> property.
        /// </summary>
        /// <param name="grid">Target grid to affect.</param>
        private void AddToGridTileObjects(Grid grid)
        {
            if (grid.TileObjects.TryGetValue(Transform.Position, out var value))
                value.Add(this);
            else
                grid.TileObjects.Add(Transform.Position, new List<TileObject> { this });
        }

        /// <summary>
        /// Add a collider property to this object.
        /// </summary>
        /// <param name="belongLayers">The collision layers to which this object belongs to.</param>
        /// <param name="collideLayers">The collision layers this object should collide with.</param>
        /// <param name="isTrigger"><c>true</c> if this collider should act as a trigger.</param>
        public void SetCollider(HashSet<string> belongLayers, HashSet<string> collideLayers, bool isTrigger = false)
        {
            Collider ??= new TileObjectCollider();
            Collider.IsTrigger = isTrigger;
            Collider.BelongLayers = belongLayers;
            Collider.CollideLayers = collideLayers;
        }

        /// <summary>
        /// Set the sprite properties for this object.
        /// </summary>
        /// <param name="path">Path to the sprite's graphic.</param>
        /// <param name="tilePivot">
        /// Position on the tile from which the object is rendered. Defined between <see cref="Vector2.Zero"/> and
        /// <see cref="Vector2.One"/> as the upper and lower bounds of the tile width. </param>
        /// <param name="imagePivot">
        /// Position on the sprite which will match positions with the <paramref name="tilePivot"/>.
        /// Defined between <see cref="Vector2.Zero"/> the pixel width and height of the sprite.</param>
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
        
        public class TileObjectCollider
        {
            public HashSet<string> BelongLayers { get; internal set; } = [];
            public HashSet<string> CollideLayers { get; internal set; } = [];
            public bool IsTrigger { get; internal set; }
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
            /// Defined between <see cref="Vector2.Zero"/> and this sprite's <see cref="Width"/> and <see cref="Height"/>. 
            /// </summary>
            public Vector2 ImagePivot { get; internal set; } = Vector2.Zero;

            public int Height { get; internal set; }
            
            public int Width { get; internal set; }
        }
    }

}
