using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects.Properties;
using Kintsugi.Objects.Sprites;
using Kintsugi.Rendering;
using Kintsugi.Tiles;
using SDL2;
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
        public TileObjectTransform Transform { get; private set; }
        /// <summary>
        /// Collision properties of this object.
        /// </summary>
        public TileObjectCollider? Collider { get; private set; }
        /// <summary>
        /// Graphic properties of this object.
        /// </summary>
        public ISpriteable? Sprite { get; private set; }
        
        /// <summary>
        /// Creates a <see cref="TileObject"/> with a default Transform property. 
        /// </summary>
        public TileObject()
        {
            Transform = new TileObjectTransform(this);
        }

        /// <summary>
        /// Establish the position of this object in a grid system. This method also updates the grid's
        /// <see cref="Grid.TileObjects"/> dictionary.
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
        /// Add a collider property to this object with these parameters.
        /// </summary>
        /// <param name="belongLayers">The collision layers to which this object belongs to.</param>
        /// <param name="collideLayers">The collision layers this object should collide with.</param>
        /// <param name="isTrigger"><c>true</c> if this collider should act as a trigger.</param>
        public void SetCollider(HashSet<string> belongLayers, HashSet<string> collideLayers, bool isTrigger = false)
        {
            Collider ??= new TileObjectCollider(this);
            Collider.IsTrigger = isTrigger;
            Collider.BelongLayers = [..belongLayers];
            Collider.CollideLayers = [..collideLayers];
        }

        /// <summary>
        /// Add a collider property to this property to be copied from another <see cref="TileObjectCollider"/>.
        /// </summary>
        /// <param name="collider">Collider object to copy from.</param>
        public void SetCollider(TileObjectCollider collider)
            => SetCollider([..collider.BelongLayers], [..collider.CollideLayers], collider.IsTrigger);

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
            Sprite ??= new TileObjectSprite(this);
            Sprite.Path = path;
            Sprite.TilePivot = tilePivot;
            Sprite.ImagePivot = imagePivot;
            
            // Get the Height and Width
            if (path == "") return;
            var image = Image.Load(path);
            Sprite.SpriteHeight = image.Height;
            Sprite.SpriteWidth = image.Width;
            image.Dispose();
        }

        /// <summary>
        /// Add a sprite to this property to be copied from another <see cref="TileObjectSprite"/>
        /// </summary>
        /// <param name="sprite">Sprite to copy from.</param>
        public void SetSprite(TileObjectSprite sprite)
            => SetSprite(sprite.Path, sprite.TilePivot, sprite.ImagePivot);

        public void SetSprite(Animation animation)
        {
            
        }
    }
    
    namespace Properties
    {
        /// <summary>
        /// Transform properties of a tile object.
        /// </summary>
        public class TileObjectTransform(TileObject parent)
        {
            /// <summary>
            /// Position of the tile object in a grid system. 
            /// </summary>
            public Vec2Int Position { get; internal set; } = Vec2Int.Zero;
            /// <summary>
            /// Direction the tile object is facing towards.
            /// </summary>
            public Facing Facing { get; internal set; } = Facing.East;
            /// <summary>
            /// Grid to which the tile object belongs to, if any.
            /// </summary>
            public Grid? Grid { get; internal set; }
            /// <summary>
            /// Layer to which the tile object belongs to in its grid, if any.
            /// </summary>
            public int Layer { get; internal set; }
            /// <summary>
            /// The object this property modifies.
            /// </summary>
            public TileObject Parent { get; } = parent;
        }
        
        /// <summary>
        /// Collision properties of a tile object.
        /// </summary>
        public class TileObjectCollider(TileObject parent)
        {
            /// <summary>
            /// Collection of layers the object belongs to.
            /// </summary>
            public HashSet<string> BelongLayers { get; internal set; } = [];
            /// <summary>
            /// Collection of layers the tile object collides with.
            /// </summary>
            public HashSet<string> CollideLayers { get; internal set; } = [];
            /// <summary>
            /// <c>true</c> if the tile object is treated as a trigger collider.
            /// </summary>
            public bool IsTrigger { get; internal set; }
            /// <summary>
            /// The object this property modifies.
            /// </summary>
            public TileObject Parent { get; } = parent;
        }
        
        /// <summary>
        /// Graphic properties of a tile object.
        /// </summary>
        public class TileObjectSprite(TileObject parent) : ISpriteable
        {
            /// <summary>
            /// File path of the tile object's sprite.
            /// </summary>
            public string Path { get; set; } = "";
            /// <summary>
            /// Position on the tile from which the object is rendered.
            /// Defined between <see cref="Vector2.Zero"/> and <see cref="Vector2.One"/> as the upper and lower bounds of the tile width.
            /// </summary>
            public Vector2 TilePivot { get; set; } = Vector2.Zero;
            /// <summary>
            /// Position on the sprite which will match positions with the <see cref="TilePivot"/>.
            /// Defined between <see cref="Vector2.Zero"/> and this sprite's <see cref="SpriteWidth"/> and <see cref="SpriteHeight"/>. 
            /// </summary>
            public Vector2 ImagePivot { get; set; } = Vector2.Zero;
            /// <summary>
            /// Height of the object in pixels.
            /// </summary>
            public int SpriteHeight { get; set; }
            /// <summary>
            /// Width of the object in pixels.
            /// </summary>
            public int SpriteWidth { get; set; }
            /// <summary>
            /// The object this property modifies.
            /// </summary>
            public TileObject Parent { get; } = parent;

            public SDL.SDL_Rect SourceRect()
                => new() {
                    x = 0,
                    y = 0,
                    w = SpriteWidth,
                    h = SpriteHeight,
                };
            
            public nint Texture => ((DisplaySDL)Bootstrap.GetDisplay()).LoadTexture(Path);
        }
    }
}
