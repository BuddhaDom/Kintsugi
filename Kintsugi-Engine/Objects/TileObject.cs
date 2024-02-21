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
        public TileObjectTransform Transform { get; private set; } = new()
        {
            GridPosition = Vec2Int.Zero,
            Facing = Facing.East,
            Grid = null,
            Layer = 0
        };
        public TileObjectCollider? Collider { get; private set; }
        public TileObjectSprite? Sprite { get; private set; }

        public void SetPosition(Vec2Int position)
        {
            Transform.Grid?.Objects[Transform.GridPosition].Remove(this);
            Transform.Grid?.Objects[];
        }
        
        public void SetCollider(bool isTrigger, IEnumerable<int> belongLayers, IEnumerable<int> collideLayers)
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
    
    public class TileObjectCollider
    {
        public HashSet<int> BelongLayers { get; set; }
        public HashSet<int> CollideLayers { get; set; }
        public bool IsTrigger { get; set; }
    }

    public class TileObjectTransform
    {
        public Vec2Int GridPosition { get; internal set; }
        public Facing Facing { get; internal set; }
        public Grid? Grid { get; internal set; }
        public int Layer { get; internal set; }
    }

    public class TileObjectSprite
    {
        public string Path { get; internal set; }
        /// <summary>
        /// Position on the tile from which the object is rendered.
        /// Defined between <see cref="Vector2.Zero"/> and <see cref="Vector2.One"/> as the upper and lower bounds of the tile width.
        /// </summary>
        public Vector2 TilePivot { get; internal set; }
        /// <summary>
        /// Position on the sprite which will match positions with the <see cref="TilePivot"/>.
        /// Defined between this sprite's <see cref="Height"/> and <see cref="Width"/>. 
        /// </summary>
        public Vector2 ImagePivot { get; internal set; }

        public int Height { get; internal set; }
        
        public int Width { get; internal set; }
        
        public TileObjectSprite(string path, Vector2 tilePivot, Vector2 imagePivot)
        {
            Path = path;
            TilePivot = tilePivot;
            ImagePivot = imagePivot;
            if (path != "")
            {
                var image = Image.Load(path);
                Height = image.Height;
                Width = image.Width;
            }
        }

        public TileObjectSprite() : this("", Vector2.Zero, Vector2.Zero) {}
    }
}
