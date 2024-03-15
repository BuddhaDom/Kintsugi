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
    internal class Levels : Level
    {
        public override string GridPath => "Assets\\Tilemaps\\Levels\\room1.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            scenario.BeginScenario();
        }
    }
}
