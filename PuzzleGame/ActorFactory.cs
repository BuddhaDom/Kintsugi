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
            character.SetCollider(["unit"], ["unit", "flying_unit", "wall", "spikes"], false);
            character.SetSpriteSingle(game.GetAssetManager().GetAssetPath("zombie.png"), Vector2.One / 2,
                new Vector2(8f, 8f));
            character.AddToGrid(grid, 2); // this should be inherited from a generic unit
            character.SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.2f);
            character.speed = 1;
            return character;
        }
        public static MovementActor Skeleton(Game game, Grid grid)
        {
            var character = new MovementActor("Skeleton");
            character.SetCollider(["unit"], ["unit", "flying_unit", "wall", "spikes"], false);
            character.SetSpriteSingle(game.GetAssetManager().GetAssetPath("skeleton.png"), Vector2.One / 2,
                new Vector2(8f, 8f));
            character.AddToGrid(grid, 2); // this should be inherited from a generic unit
            character.SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.2f);
            character.speed = 2;
            return character;
        }

        public static MovementActor Mummy(Game game, Grid grid)
        {
            var character = new MovementActor("Mummy");
            character.SetCollider(["unit"], ["unit", "flying_unit", "wall", "spikes"], false);
            character.SetSpriteSingle(game.GetAssetManager().GetAssetPath("mummy.png"), Vector2.One / 2,
                new Vector2(8f, 8f));
            character.AddToGrid(grid, 2); // this should be inherited from a generic unit
            character.SetEasing(TweenSharp.Animation.Easing.BounceEaseOut, 0.2f);
            character.speed = 1;
            character.reverse_movement = -1;
            return character;
        }

        public static MovementActor Ghost(Game game, Grid grid)
        {
            var character = new MovementActor("Ghost");
            character.SetCollider(["flying_unit"], ["void", "unit"], false);
            character.SetSpriteSingle(game.GetAssetManager().GetAssetPath("ghost.png"), Vector2.One / 2,
                new Vector2(8f, 8f));
            character.AddToGrid(grid, 2); // this should be inherited from a generic unit
            character.SetEasing(TweenSharp.Animation.Easing.ExpoEaseInOut, 0.2f);
            character.speed = 1;
            return character;
        }
        public static Actor Goal(Game game, Grid grid)
        {
            var character = new BaseActor();
            character.SetSpriteSingle(game.GetAssetManager().GetAssetPath("portal.png"), Vector2.One / 2,
                new Vector2(8f, 8f));
            character.SetCollider([], ["unit", "flying_unit"], false);

            character.AddToGrid(grid, 2);
            return character;
        }

    }
}
