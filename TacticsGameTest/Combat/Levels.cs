using Kintsugi.Core;
using System.Numerics;

namespace TacticsGameTest.Rooms
{
    internal class Room0 : Level
    {
        public override string GridPath => @"Tilemaps\Levels\room0.tmx";
        
        public override void SetUp()
        {
            InitEnemy(ActorFactory.Archer, 2, 1);
            InitEnemy(ActorFactory.Grunt, 5, 2);
            
            InitPlayerCharacters(5,7,6,8,4,9);
        }
    }
    
    internal class Room1 : Level
    {
        public override string GridPath => @"Tilemaps\Levels\room1.tmx";

        public override void SetUp()
        {
            InitEnemy(ActorFactory.Grunt, 1, 3);

            InitPlayerCharacters(2,0,0,0,4,0);
        }
    }

    internal class Room2 : Level
    {
        public override string GridPath => @"Tilemaps\Levels\room2.tmx";

        public override void SetUp()
        {
            InitEnemy(ActorFactory.Archer, 4, 4);
            InitEnemy(ActorFactory.Grunt, 9, 2);
            InitEnemy(ActorFactory.Grunt, 0, 2);

            InitPlayerCharacters(6, 9, 3, 9, 5, 9);
        }
    }
}
