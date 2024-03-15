using Kintsugi.Audio;
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


            scenario.BeginScenario();
        }
    }
}
