using Kintsugi.Core;
using System.Numerics;

namespace TacticsGameTest.Rooms
{
    internal class Room1 : Level
    {
        public override string GridPath => @"Tilemaps\Levels\room1.tmx";

        public override void SetUp()
        {

            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var character = ActorFactory.Grunt(grid);
            character.SetPosition(new Vec2Int(1, 3), false);

            group_enemy.AddActor(character);

            spearCharacter.SetPosition(new Vec2Int(0, 0), false);
            tankCharacter.SetPosition(new Vec2Int(2, 0), false);
            rogueCharacter.SetPosition(new Vec2Int(4, 0), false);
        }
    }

    internal class Room2 : Level
    {
        public override string GridPath => @"Tilemaps\Levels\room2.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var sniper = ActorFactory.Archer(grid);
            sniper.SetPosition(new Vec2Int(4,4), false);
            group_enemy.AddActor(sniper);
            
            var grunt1 = ActorFactory.Grunt(grid);
            grunt1.SetPosition(new Vec2Int(9,2), false);
            group_enemy.AddActor(grunt1);
            
            var grunt2 = ActorFactory.Grunt(grid);
            grunt2.SetPosition(new Vec2Int(0,2), false);
            group_enemy.AddActor(grunt2);
            
            spearCharacter.SetPosition(new Vec2Int(3, 9), false);
            tankCharacter.SetPosition(new Vec2Int(6, 9), false);
            rogueCharacter.SetPosition(new Vec2Int(5, 9), false);
        }
    }
}
