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
using TacticsGameTest.Units;

namespace TacticsGameTest.Rooms
{
    internal abstract class Level
    {
        public GridBase grid;
        public CombatScenario scenario;

        public PlayerControlGroup group_player;
        public EnemyControlGroup group_enemy;

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

            SetUp();
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

        }
    }
}
