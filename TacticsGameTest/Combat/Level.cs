using Kintsugi.Core;
using Kintsugi.Input;
using System.Drawing;
using System.Numerics;
using Kintsugi.EventSystem;
using Kintsugi.Tiles;
using TacticsGameTest.Combat;
using TacticsGameTest.UI;
using TacticsGameTest.Units;
using PuzzleGame;
using Kintsugi.Objects;

namespace TacticsGameTest.Rooms
{
    internal abstract class Level
    {
        public GridBase grid;
        public CombatScenario scenario;

        public PlayerControlGroup group_player;
        public List<EnemyControlGroup> enemyGroups = new();

        public PlayerActor spearCharacter;
        public PlayerActor tankCharacter;
        public PlayerActor rogueCharacter;


        public abstract string GridPath { get; }
        public void AddEnemy(Actor actor)
        {
            var group_enemy = new EnemyControlGroup("ENEMY");
            group_enemy.AddActor(actor);
            scenario.AddControlGroup(group_enemy);
            enemyGroups.Add(group_enemy);


        }
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

            scenario.AddControlGroup(group_player);

            spearCharacter = ActorFactory.SpearPlayer(grid);
            tankCharacter = ActorFactory.TankPlayer(grid);
            rogueCharacter = ActorFactory.RoguePlayer(grid);

            grid.Layers[3].SwitchColliderType<SpikeCollider>();

            group_player.AddActor(spearCharacter);
            group_player.AddActor(tankCharacter);
            group_player.AddActor(rogueCharacter);

            Bootstrap.GetCameraSystem().Size = 4 * 24;
            Bootstrap.GetCameraSystem().Position = new Vector2(85, 75);

            SetUp();
            scenario.BeginScenario();
            
            foreach (var item in group_player.GetActors())
            {
                scenario.players.Add((CombatActor)item);
                AddLevelInputListener((IInputListener)item);
            }
            foreach (var group in enemyGroups)
            {
                foreach (var actor in group.GetActors())
                {
                    scenario.enemies.Add((CombatActor)actor);
                }
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
        
        protected void InitEnemy(Func<Grid, CombatActor> factoryMethod, Vec2Int position)
        {
            var character = factoryMethod(grid);
            character.SetPosition(position, false);
            group_enemy.AddActor(character);
        }

        protected void InitEnemy(Func<Grid, CombatActor> factoryMethod, int x, int y)
            => InitEnemy(factoryMethod, new Vec2Int(x, y));

        protected void InitPlayerCharacters(Vec2Int tankPos, Vec2Int spearPos, Vec2Int roguePos)
        {
            tankCharacter.SetPosition(tankPos, false);
            spearCharacter.SetPosition(spearPos, false);
            rogueCharacter.SetPosition(roguePos, false);
        }

        protected void InitPlayerCharacters(int tankPosX,
            int tankPosY,
            int spearPosX,
            int spearPosY,
            int roguePosX,
            int roguePosY)
            => InitPlayerCharacters(
                new Vec2Int(tankPosX, tankPosY), 
                new Vec2Int(spearPosX, spearPosY),
                new Vec2Int(roguePosX, roguePosY));
    }
}
