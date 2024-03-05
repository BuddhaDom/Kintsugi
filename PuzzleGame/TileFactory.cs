using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Objects.Properties;
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
