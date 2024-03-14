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
using TacticsGameTest;

namespace TacticsGameTest
{
    internal abstract class Level
    {
        public Grid grid;
        public MovingScenario scenario;

        public UnitControlGroup group_player;
        public EnemyControlGroup group_environment;

        public Game game;

        public abstract string GridPath { get; }
        public void Load(Game game)
        {
            this.game = game;


            grid = new Grid(game.GetAssetManager().GetAssetPath(GridPath), gridVisible: true, gridColor: Color.DarkBlue);
            grid.Position.X = 0;
            grid.Position.Y = 0;

            grid.Layers[2].SwitchColliderType<SpikeCollider>();

            scenario = new MovingScenario();

            group_player = new UnitControlGroup("PLAYER");
            group_environment = new EnemyControlGroup("ENVIRONMENT");

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
