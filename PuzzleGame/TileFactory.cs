using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Tiles;
using System.Numerics;

namespace PuzzleGame
{
    static class TileFactory
    {
        public static TileObject Wall(Game game, Grid grid, int layer)
        {
            var character = new TileObject();
            character.SetSpriteSingle(game.GetAssetManager().GetAssetPath("guy.png"), Vector2.One / 2,
                new Vector2(6.5f, 8.5f));
            character.AddToGrid(grid, layer);
            return character;
        }
    }
}
