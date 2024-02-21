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
            grid = new Grid(game.GetAssetManager().GetAssetPath("TiledTesting\\forestpath.tmx"), gridVisible: true, gridColor: Color.DarkBlue);
            grid.Transform.X = 0;
            grid.Transform.Y = 0;
            Bootstrap.GetCameraSystem().Size = 16 * 10;


            // TODO: This sucks. Should be attached to grid on the transform itself.
            var transform = new TileObjectTransform(Vec2Int.One * 3, 0, grid);
            var transform2 = new TileObjectTransform(new Vec2Int(1, 2), 0, grid);
            var collider = new TileObjectCollider([0], [1]);
            var sprite = new TileObjectSprite(game.GetAssetManager().GetAssetPath("guy.png"), Vector2.One / 2,
                new Vector2(6.5f, 8.5f));
            var character = new MovementActor(transform, collider, sprite, "john");
            var character2 = new MovementActor(transform2, collider, sprite, "bob");
            var scenario = new MovingScenario();
            var group_player = new PlayerControlGroup("PLAYER")
            var group_environment = new EnvironmentControlGroup("ENVIRONMENT");

            group.AddActor(character);
            group.AddActor(character2);

            scenario.AddControlGroup(group);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
}
