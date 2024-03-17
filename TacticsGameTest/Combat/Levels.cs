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
            var playerchar = ActorFactory.PlayerCharacter(grid);

            group_player.AddActor(playerchar);
            group_enemy.AddActor(character);


            scenario.BeginScenario();
        }
    }
}
