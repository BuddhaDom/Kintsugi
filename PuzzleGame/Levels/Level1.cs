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
        public override void Load(Game game)
        {
            grid = new Grid(game.GetAssetManager().GetAssetPath("Tiles\\level1.tmx"), gridVisible: true, gridColor: Color.DarkBlue);
            grid.Transform.X = 0;
            grid.Transform.Y = 0;
            Bootstrap.GetCameraSystem().Size = 16 * 10;

            grid.Layers[2].SwitchColliderType<SpikeCollider>();

            // TODO: This sucks. Should be attached to grid on the transform itself.
            var character = ActorFactory.Zombie(game, grid, 3);
            character.SetPosition(new Vec2Int(0, 5));
            var scenario = new MovingScenario();
            var group_player = new PlayerControlGroup("PLAYER");
            var group_environment = new EnvironmentControlGroup("ENVIRONMENT");

            group_player.AddActor(character);
            //group.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
}
