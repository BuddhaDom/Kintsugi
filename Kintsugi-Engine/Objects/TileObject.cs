using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Rendering;
using Kintsugi.Tiles;

namespace Kintsugi.Objects
{
    public class TileObject
    {
        public TileObjectCollider? Collider { get; set; }
        public TileObjectTransform Transform { get; set; }
        public TileObjectSprite? Sprite { get; set; }

        public TileObject()
        {
            Collider = null;
            Transform = new TileObjectTransform();
            Sprite = null;
        }
    }
    
    public class TileObjectCollider
    {
        public HashSet<int> BelongLayers { get; set; }
        public HashSet<int> CollideLayers { get; set; }
        public bool IsTrigger { get; set; }

        public TileObjectCollider(HashSet<int> belongLayers, HashSet<int> collideLayers, bool isTrigger)
        {
            BelongLayers = belongLayers;
            CollideLayers = collideLayers;
            IsTrigger = isTrigger;
        }
        
        public TileObjectCollider() : this([],[], false) {}
    }

    public class TileObjectTransform
    {
        public Grid? Grid { get; set; }
        public Vec2Int GridPosition { get; set; }
        public Facing Facing { get; set; }

        public TileObjectTransform(Grid? grid, Vec2Int gridPosition, Facing facing)
        {
            Grid = grid;
            GridPosition = gridPosition;
            Facing = facing;
        }

        public TileObjectTransform() : this(null,Vec2Int.Zero, Facing.East) {}
    }

    public class TileObjectSprite
    {
        internal nint Sprite { get; }
        public string SpritePath { get; }
        public Vector2 Pivot { get; }

        public TileObjectSprite(string spritePath, Vector2 pivot)
        {
            SpritePath = spritePath;
            Pivot = pivot;
            if(spritePath != "")
                Sprite = ((DisplaySDL)Bootstrap.GetDisplay()).LoadTexture(spritePath);
        }

        public TileObjectSprite() : this("", Vector2.Zero) {}
    }
}
