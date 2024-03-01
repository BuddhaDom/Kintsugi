using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    static class ActorFactory
    {
        public static MovementActor Zombie(Game game, Grid grid)
        {
            var character = new MovementActor("Zombie");
            character.SetCollider(["unit"], ["unit", "wall", "spikes"], false);
            character.SetSprite(game.GetAssetManager().GetAssetPath("zombie.png"), Vector2.One / 2,
                new Vector2(6.5f, 8.5f));
            character.AddToGrid(grid, 2); // this should be inherited from a generic unit
            character.speed = 1;
            return character;
        }
        public static MovementActor Skeleton(Game game, Grid grid)
        {
            var character = new MovementActor("Skeleton");
            character.SetCollider(["unit"], ["unit", "wall", "spikes"], false);
            character.SetSprite(game.GetAssetManager().GetAssetPath("skeleton.png"), Vector2.One / 2,
                new Vector2(6.5f, 8.5f));
            character.AddToGrid(grid, 2); // this should be inherited from a generic unit
            character.speed = 2;
            return character;
        }
        public static Actor Goal(Game game, Grid grid)
        {
            var character = new BaseActor();
            character.SetSprite(game.GetAssetManager().GetAssetPath("portal.png"), Vector2.One / 2,
                new Vector2(6.5f, 8.5f)); 
            character.SetCollider([], ["unit"], false);

            character.AddToGrid(grid, 2); 
            return character;
        }

    }
}
