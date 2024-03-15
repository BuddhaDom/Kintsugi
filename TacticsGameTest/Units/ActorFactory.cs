using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Objects.Graphics;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest
{
    static class ActorFactory
    {
        static Animation zombieAnim ;
        static Animation skeletonAnim;
        static Animation mummyAnim;

        public static SelectableActor Zombie(Game game, Grid grid)
        {
            if (zombieAnim == null)
            {
                var spritesheet = new SpriteSheet(
                    game.GetAssetManager().GetAssetPath("MutilatedStumbler.png"),
                    16,
                    16,
                    4,
                    Vector2.One / 2,
                    new Vector2(8f, 8f)
                    );

                zombieAnim = new Animation(
                    1f,
                    spritesheet,
                    Enumerable.Range(0, 4));
            }


            var character = new MovementActor("Zombie");

            character.SetAnimation(zombieAnim);
            character.SetCollider(["unit"], ["unit", "flying_unit", "wall", "spikes"], false);
            character.AddToGrid(grid, 2); // this should be inherited from a generic unit
            character.SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.2f);
            character.speed = 1;
            return character;
        }

    }
}
