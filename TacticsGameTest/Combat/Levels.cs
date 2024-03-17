using Kintsugi.Core;
using System.Numerics;

namespace TacticsGameTest.Rooms
{
    internal class RoomIntro : Level
    {
        public override string GridPath => @"Tilemaps\Levels\roomIntro.tmx";
        
        public override void SetUp()
        {
            InitEnemy(ActorFactory.Archer, 2, 1);
            InitEnemy(ActorFactory.Grunt, 5, 2);
            
            InitPlayerCharacters(5,7,6,8,4,9);
        }
    }

    internal class RoomTower : Level
    {
        public override string GridPath => @"Tilemaps\Levels\roomTower.tmx";

        public override void SetUp()
        {
            InitEnemy(ActorFactory.Archer, 4, 4);
            InitEnemy(ActorFactory.Grunt, 9, 2);
            InitEnemy(ActorFactory.Grunt, 0, 2);

            InitPlayerCharacters(6, 9, 3, 9, 5, 9);
        }
    }
    
    internal class RoomMine : Level
    {
        public override string GridPath => @"Tilemaps\Levels\roomMine.tmx";

        public override void SetUp()
        {
            InitEnemy(ActorFactory.Executioner, 3,1);
            InitEnemy(ActorFactory.Executioner, 4,2);
            InitEnemy(ActorFactory.Executioner, 5,1);

            InitPlayerCharacters(5,7,6,8,4,9);
        }
    }

    internal class RoomAmbush : Level
    {
        public override string GridPath => @"Tilemaps\Levels\roomAmbush.tmx";

        public override void SetUp()
        {
            InitEnemy(ActorFactory.Ninja, 2,2);
            InitEnemy(ActorFactory.Ninja, 14,2);
            InitEnemy(ActorFactory.Ninja, 2,14);
            InitEnemy(ActorFactory.Ninja, 14,14);
            InitEnemy(ActorFactory.Grunt, 1,8);
            InitEnemy(ActorFactory.Grunt, 15,8);
            
            InitPlayerCharacters(7,8,9,8,8,8);
        }
    }

    internal class RoomBoss : Level
    {
        public override string GridPath => @"Tilemaps\Levels\roomBoss.tmx";

        public override void SetUp()
        {
            InitEnemy(ActorFactory.Executioner, 0, 8);
            InitEnemy(ActorFactory.Ninja,9,8);
            InitEnemy(ActorFactory.Grunt,0,12);
            InitEnemy(ActorFactory.Archer, 9,12);
            
            
            InitPlayerCharacters(5,13,4,14,5,14);
        }
    }
}
