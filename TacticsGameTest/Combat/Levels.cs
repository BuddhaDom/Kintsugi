using Kintsugi.Core;
using System.Numerics;

namespace TacticsGameTest.Rooms
{
    internal class Room1 : Level
    {
        public override string GridPath => "Tilemaps\\Levels\\room1.tmx";

        public override void SetUp()
        {

            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var character = ActorFactory.Grunt(grid);
            character.SetPosition(new Vec2Int(1, 3), false);

            group_enemy.AddActor(character);

            playerChar1.SetPosition(new Vec2Int(0, 0), false);
            playerChar2.SetPosition(new Vec2Int(2, 0), false);
            playerChar3.SetPosition(new Vec2Int(4, 0), false);

            scenario.BeginScenario();
        }
    }
}
