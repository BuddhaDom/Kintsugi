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
        public HashSet<string> BelongLayers { get; set; }
        public HashSet<string> CollideLayers { get; set; }
        public bool IsTrigger { get; set; }

        public TileObjectCollider(HashSet<string> belongLayers, HashSet<string> collideLayers, bool isTrigger = false)
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
        /// <summary>
        /// Position on the tile from which the object is rendered.
        /// Defined between <see cref="Vector2.Zero"/> and <see cref="Vector2.One"/> as the upper and lower bounds of the tile width.
        /// </summary>
        public Vector2 TilePivot { get; }
        /// <summary>
        /// Position on the sprite which will match positions with the <see cref="TilePivot"/>.
        /// Defined between this sprite's <see cref="Height"/> and <see cref="Width"/>. 
        /// </summary>
        public Vector2 ImagePivot { get; set; }

        public int Height { get; }
        
        public int Width { get; }
        
        public TileObjectSprite(string spritePath, Vector2 tilePivot, Vector2 imagePivot)
        {
            SpritePath = spritePath;
            TilePivot = tilePivot;
            ImagePivot = imagePivot;
            if (spritePath != "")
            {
                Sprite = ((DisplaySDL)Bootstrap.GetDisplay()).LoadTexture(spritePath);
                var image = Image.Load(spritePath);
                Height = image.Height;
                Width = image.Width;
            }
        }

        public TileObjectSprite() : this("", Vector2.Zero, Vector2.Zero) {}
    }
}
