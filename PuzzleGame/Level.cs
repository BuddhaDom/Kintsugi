using Kintsugi.Audio;
using Kintsugi.Core;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    internal abstract class Level
    {
        public Grid grid;
        public MovingScenario scenario;

        public PlayerControlGroup group_player;
        public EnvironmentControlGroup group_environment;

        public Game game;

        public abstract string GridPath { get; }
        public void Load(Game game)
        {
            this.game = game;


            grid = new Grid(game.GetAssetManager().GetAssetPath(GridPath), gridVisible: true, gridColor: Color.DarkBlue);
            grid.Transform.X = 0;
            grid.Transform.Y = 0;

            grid.Layers[2].SwitchColliderType<SpikeCollider>();

            scenario = new MovingScenario();

            group_player = new PlayerControlGroup("PLAYER");
            group_environment = new EnvironmentControlGroup("ENVIRONMENT");

            SetUp();
        }
        public abstract void SetUp();
        public void Unload()
        {
            grid.ToBeDestroyed = true;
            grid = null;
            scenario.EndScenario();
            scenario = null;
        }
    }
}
