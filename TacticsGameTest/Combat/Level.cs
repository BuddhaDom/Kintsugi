using Kintsugi.Core;
using Kintsugi.Input;
using System.Drawing;
using Kintsugi.EventSystem;
using TacticsGameTest.Combat;
using TacticsGameTest.UI;
using TacticsGameTest.Units;
using PuzzleGame;

namespace TacticsGameTest.Rooms
{
    internal abstract class Level
    {
        public GridBase grid;
        public CombatScenario scenario;

        public PlayerControlGroup group_player;
        public EnemyControlGroup group_enemy;

        public PlayerActor spearCharacter;
        public PlayerActor tankCharacter;
        public PlayerActor rogueCharacter;


        public abstract string GridPath { get; }
        public void Load()
        {
            var game = Bootstrap.GetRunningGame();

            Audio.I.music.Start();


            grid = new GridBase(game.GetAssetManager().GetAssetPath(GridPath), gridVisible: true, gridColor: Color.DarkBlue);

            Bootstrap.GetInput().AddListener(grid);
            grid.Position.X = 0;
            grid.Position.Y = 0;
           
            scenario = new CombatScenario();

            group_player = new PlayerControlGroup("PLAYER");
            group_enemy = new EnemyControlGroup("ENEMY");

            scenario.AddControlGroup(group_player);
            scenario.AddControlGroup(group_enemy);

            spearCharacter = ActorFactory.SpearPlayer(grid);
            tankCharacter = ActorFactory.TankPlayer(grid);
            rogueCharacter = ActorFactory.RoguePlayer(grid);

            grid.Layers[2].SwitchColliderType<SpikeCollider>();

            group_player.AddActor(spearCharacter);
            group_player.AddActor(tankCharacter);
            group_player.AddActor(rogueCharacter);


            SetUp();
            scenario.BeginScenario();
            
            foreach (var item in group_player.GetActors())
            {
                scenario.players.Add((CombatActor)item);
                AddLevelInputListener((IInputListener)item);
            }
            foreach (var item in group_enemy.GetActors())
            {
                scenario.enemies.Add((CombatActor)item);
            }
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
            foreach (var item in group_player.GetActors())
            {
                ((PlayerActor)item).ActorUI.Destroy();

                ((PlayerActor)item).ActorUI = null;
            }
            Bootstrap.GetInput().RemoveListener(grid);
            grid.Destroy();
            grid = null;
            scenario.EndScenario();
            scenario = null;
            EventManager.I.ClearQueue();
            foreach (var listener in levelInputListeners)
            {
                Bootstrap.GetInput().RemoveListener(listener);
            }
            Audio.I.music.Stop();
            HUD.Instance.Clear();
        }
    }
}
