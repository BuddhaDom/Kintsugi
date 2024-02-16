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
        public TileObjectTransform Transform { get; set; }
        public TileObjectCollider? Collider { get; set; }
        public TileObjectSprite? Sprite { get; set; }

        public TileObject(TileObjectTransform transform, TileObjectCollider? collider = null, TileObjectSprite? sprite = null)
        {
            Transform = transform;
            Collider = collider;
            Sprite = sprite;

            transform.Grid?.Objects.Add(this);
        }

        public TileObject() : this(new TileObjectTransform()) {}
    }
    
    public class TileObjectCollider
    {
        public HashSet<int> BelongLayers { get; set; }
        public HashSet<int> CollideLayers { get; set; }
        public bool IsTrigger { get; set; }

        public TileObjectCollider(HashSet<int> belongLayers, HashSet<int> collideLayers, bool isTrigger = false)
        {
            BelongLayers = belongLayers;
            CollideLayers = collideLayers;
            IsTrigger = isTrigger;
        }
        
        public TileObjectCollider() : this([],[]) {}
    }

    public class TileObjectTransform
    {
        public Grid? Grid { get; set; }
        public Vec2Int GridPosition { get; set; }
        public int Layer { get; set; }
        public Facing Facing { get; set; }

        public TileObjectTransform(Vec2Int gridPosition, int layer = 0, Grid? grid = null, Facing facing = Facing.East)
        {
            Grid = grid;
            GridPosition = gridPosition;
            Layer = layer;
            Facing = facing;
        }

        public TileObjectTransform() : this(Vec2Int.Zero) {}
    }

    public class TileObjectSprite
    {
        internal nint Sprite { get; }
        public string SpritePath { get; }
        public Vector2 Pivot { get; }

        public int Height { get; }
        
        public int Width { get; }
        public TileObjectSprite(string spritePath, Vector2 pivot)
        {
            SpritePath = spritePath;
            Pivot = pivot;
            if (spritePath != "")
            {
                Sprite = ((DisplaySDL)Bootstrap.GetDisplay()).LoadTexture(spritePath);
                var image = Image.Load(spritePath);
                Height = image.Height;
                Width = image.Width;
            }
        }

        public TileObjectSprite() : this("", Vector2.Zero) {}
    }
}
