using Kintsugi.Core;
using System.Numerics;
using TacticsGameTest.Map;
using TacticsGameTest.Rooms;
using TacticsGameTest.Units;

namespace TacticsGameTest
{
    internal class MapManagement
    {
        static MapManagement _instance;

        private Dictionary<Vector2, Level> rooms = new Dictionary<Vector2, Level>()
            {
                {new Vec2Int(2,8), new Room2() },
                {new Vec2Int(6,7), new Room2() },
                {new Vec2Int(5,0), new Room2() }
            };
        private HashSet<Vector2> heals = new HashSet<Vector2>
            {
                new Vec2Int(4,8),
                new Vec2Int(7,4),
                new Vec2Int(2,4)
            };
        private HashSet<Vector2> keys = new HashSet<Vector2>
            {
                new Vec2Int(11,8),
                new Vec2Int(3,3)
            };
        public static MapManagement I
            {
            get
            {
                if (_instance == null)
                {
                    _instance = new MapManagement();
                }
                return _instance;
            }
            }

        public Map.Map OverworldMap;
        public MapScenario OverworldScenario;
        private void SetupOverworld()
        {
            OverworldMap = new Map.Map();
            OverworldMap.Load();

            var party = new PartyActor("FantasyBattlePack\\SwordFighter\\LongHair\\Blue1.png");
            party.AddToGrid(OverworldMap.grid, 4);
            party.SetPosition(new Vec2Int(5, 9));



            OverworldScenario = new MapScenario();

            var testControlGroup = new MapControlGroup();

            OverworldScenario.AddControlGroup(testControlGroup);

            testControlGroup.AddActor(party);
            OverworldScenario.BeginScenario();
        }
        public void LoadOverworld()
        {
            if (OverworldMap == null)
            {
                SetupOverworld();
            }
            currentRoom?.Unload();
            currentRoom = null;

            Bootstrap.GetInput().AddListener(OverworldMap.grid);
            OverworldMap.grid.Position = new Vector2(0, 0);
            Audio.I.musicExplore.Start();
        }
        public void UnloadOverworld()
        {
            Bootstrap.GetInput().RemoveListener(OverworldMap.grid);
            OverworldMap.grid.Position = new Vector2(10000, 10000);
            Audio.I.musicExplore.Stop();

        }
        public Level currentRoom { get; private set; }
            // dont touch
        public void EnterArea(Vec2Int pos)
        {
            if (rooms.TryGetValue(pos, out var room))
            {
                if (CheckBossRoom(pos)) return;
                LoadRoom(room);
            }
            if (heals.Contains(pos))
            {
                Audio.I.PlayAudio("ConfirmJingle");
            }
            if (keys.Contains(pos))
            {
                PlayerCharacterData.keys += 1;
                keys.Remove(pos);
                Audio.I.PlayAudio("KeysJingle");
            }

        }

        private bool CheckBossRoom(Vec2Int pos) // Check if locked when on it
        {
            if (pos.x == 5 && pos.y == 0) // boss room coordinates
            {
                if (PlayerCharacterData.keys >= 2)
                {
                    Audio.I.PlayAudio("DoorOpen");
                    return false;
                }
                else
                {
                    Audio.I.PlayAudio("DoorLocked");
                    return true;
                }
            }
            return false;

        }
        private void LoadRoom(Level level)
        {
            currentRoom = level;
            UnloadOverworld();
            level.Load();
        }
    }
}
