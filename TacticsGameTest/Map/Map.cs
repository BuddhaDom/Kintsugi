using Kintsugi.Core;
using System.Drawing;

namespace TacticsGameTest.Map
{
    internal class Map
    {
        public GridBase grid;

        public MapScenario scenario;

        public UnitControlGroup group_player;
        public string GridPath => "Tilemaps\\Levels\\map.tmx";
        public void Load()
        {
            var game = Bootstrap.GetRunningGame();


            grid = new GridBase(game.GetAssetManager().GetAssetPath(GridPath), gridVisible: false, gridColor: Color.Red);
            grid.Position.X = 0;
            grid.Position.Y = 0;

            var scenario = new MapScenario();

            group_player = new UnitControlGroup("PLAYER");
            scenario.AddControlGroup(group_player);

            
        }
        public void Unload()
        {
        }
    }
}
