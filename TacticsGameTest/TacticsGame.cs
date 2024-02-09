using Kintsugi.Core;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest
{
    internal class TacticsGame : Game
    {
        private GameObject grid;

        public override void Initialize()
        {
            grid = new Grid(GetAssetManager().GetAssetPath("TiledTesting\\forestpath.tmx"), gridVisible: true, gridColor: Color.DarkBlue);
            grid.Transform.X = 70;
            grid.Transform.Y = 70;

        }

        public override void Update()
        {
            Bootstrap.GetDisplay().ShowText("FPS: " + Bootstrap.GetSecondFPS() + " / " + Bootstrap.GetFPS(), 10, 10, 12, 255, 255, 255);

        }
    }
}
