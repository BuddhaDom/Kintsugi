using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Tiles;
using System.Drawing;
using Kintsugi.EventSystem;

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
            grid.Position.X = 0;
            grid.Position.Y = 0;

            grid.Layers[2].SwitchColliderType<SpikeCollider>();

            scenario = new MovingScenario();

            group_player = new PlayerControlGroup("PLAYER");
            group_environment = new EnvironmentControlGroup("ENVIRONMENT");

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
