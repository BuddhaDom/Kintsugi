using Engine.EventSystem;
using Kintsugi.Audio;
using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Combat;

namespace TacticsGameTest.Rooms
{
    internal abstract class Level
    {
        public Grid grid;
        public CombatScenario scenario;

        public UnitControlGroup group_player;
        public UnitControlGroup group_enemy;

        public abstract string GridPath { get; }
        public void Load()
        {
            var game = Bootstrap.GetRunningGame();


            grid = new Grid(game.GetAssetManager().GetAssetPath(GridPath), gridVisible: true, gridColor: Color.DarkBlue);
            grid.Position.X = 0;
            grid.Position.Y = 0;
           
            scenario = new CombatScenario();

            group_player = new UnitControlGroup("PLAYER");
            group_enemy = new UnitControlGroup("ENEMY");

            scenario.AddControlGroup(group_player);
            scenario.AddControlGroup(group_enemy);

            SetUp();
        }
        public abstract void SetUp();
        private List<IInputListener> levelInputListeners = new();
        public void AddLevelInputListener(IInputListener listener)
        {
            levelInputListeners.Add(listener);
            Bootstrap.GetInput().AddListener(listener);
        }
        public void Unload()
        {
            grid.Destroy();
            grid = null;
            scenario.EndScenario();
            scenario = null;
            EventManager.I.ClearQueue();
            foreach (var listener in levelInputListeners)
            {
                Bootstrap.GetInput().RemoveListener(listener);
            }
        }
    }
}
