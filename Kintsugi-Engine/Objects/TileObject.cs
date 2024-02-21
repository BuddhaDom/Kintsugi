using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Rendering;
using Kintsugi.Tiles;
using SixLabors.ImageSharp;

namespace Kintsugi.Objects
{
    public class TileObject
    {
        public TileObjectTransform Transform { get; private set; } = new();
        public TileObjectCollider? Collider { get; private set; }
        public TileObjectSprite? Sprite { get; private set; }

        public void SetPosition(Vec2Int position)
        {
            if (Transform.Grid != null)
            {
                AddToGridTileObjects(Transform.Grid);
                RemoveFromGridTileObjects(Transform.Grid);
            }
            Transform.Position = position;
        }

        public void Move(Vec2Int vector)
            => SetPosition(Transform.Position + Transform.Position);

        public void RemoveFromGrid()
        {
            if (Transform.Grid == null) return;
            RemoveFromGridTileObjects(Transform.Grid);
            Transform.Grid = null;
        }

        private void RemoveFromGridTileObjects(Grid grid)
        {
            grid.TileObjects[Transform.Position].Remove(this);
            if (grid.TileObjects[Transform.Position].Count == 0)
                grid.TileObjects.Remove(Transform.Position);
        }

        public void AddToGrid(Grid grid, int layer = 0)
        {
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

        public void SetCollider(HashSet<int> belongLayers, HashSet<int> collideLayers, bool isTrigger = false)
        {
            if (Collider == null)
                Collider = new TileObjectCollider
                {
                    IsTrigger = isTrigger,
                    BelongLayers = (HashSet<int>)belongLayers,
                    CollideLayers = (HashSet<int>)collideLayers
                };
            else
            {
                Collider.IsTrigger = isTrigger;
                Collider.BelongLayers = (HashSet<int>)belongLayers;
                Collider.CollideLayers = (HashSet<int>)collideLayers;
            }
        }

        public void SetSprite(string path, Vector2 tilePivot = default, Vector2 imagePivot = default)
        {
            if (Sprite == null)
                Sprite = new TileObjectSprite
                {
                    Path = path,
                    TilePivot = tilePivot,
                    ImagePivot = imagePivot
                };
            else
            {
                Sprite.Path = path;
                Sprite.TilePivot = tilePivot;
                Sprite.ImagePivot = imagePivot;
            }

            if (path == "") return;
            var image = Image.Load(path);
            Sprite.Height = image.Height;
            Sprite.Width = image.Width;
            image.Dispose();
        }
    }
    
    public class TileObjectTransform
    {
        public Vec2Int Position { get; internal set; } = Vec2Int.Zero;
        public Facing Facing { get; internal set; } = Facing.East;
        public Grid? Grid { get; internal set; }
        public int Layer { get; internal set; }
    }
    
    public class TileObjectCollider
    {
        public HashSet<int> BelongLayers { get; internal set; } = new();
        public HashSet<int> CollideLayers { get; internal set; } = new();
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
        /// Defined between this sprite's <see cref="Height"/> and <see cref="Width"/>. 
        /// </summary>
        public Vector2 ImagePivot { get; internal set; } = Vector2.Zero;

        public int Height { get; internal set; }
        
        public int Width { get; internal set; }
    }
}
