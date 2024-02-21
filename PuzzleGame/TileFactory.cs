using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    static class TileFactory
    {
        public static TileObject Wall(Game game)
        {
            var transform = new TileObjectTransform(Vec2Int.One * 3, 0);
            var collider = new TileObjectCollider([0], [1]);
            var sprite = new TileObjectSprite(game.GetAssetManager().GetAssetPath("guy.png"), Vector2.One / 2,
                new Vector2(6.5f, 8.5f));
            var character = new TileObject(transform, collider, sprite);
            return character;
        }
    }
}
