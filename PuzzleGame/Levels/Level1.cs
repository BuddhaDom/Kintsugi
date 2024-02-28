using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.Levels
{
    internal class Level1 : Level
    {
        public override string GridPath => "Tiles\\level1.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 16 * 10;

            

            // TODO: This sucks. Should be attached to grid on the transform itself.
            var character = ActorFactory.Zombie(game, grid);
            character.SetPosition(new Vec2Int(0, 5));
            var scenario = new MovingScenario();
            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
            };
            scenario.goals[0].SetPosition(new Vec2Int(4, 0));
            group_player.AddActor(character);
            //group.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
}
