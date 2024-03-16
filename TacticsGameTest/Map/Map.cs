using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Combat;

namespace TacticsGameTest.Map
{
    internal class Map
    {
        public Grid grid;

        public MapScenario scenario;

        public UnitControlGroup group_player;
        public string GridPath => "Tilemaps\\Levels\\map.tmx";
        public void Load()
        {
            var game = Bootstrap.GetRunningGame();


            grid = new Grid(game.GetAssetManager().GetAssetPath(GridPath), gridVisible: true, gridColor: Color.Red);
            grid.Position.X = 0;
            grid.Position.Y = 0;

            var scenario = new MapScenario();

            group_player = new UnitControlGroup("PLAYER");
            scenario.AddControlGroup(group_player);

            
        }
    }
}
