using Kintsugi.Core;
using Kintsugi.Objects;
using System.Numerics;
using TacticsGameTest.Map;
using TacticsGameTest.Rooms;
using TacticsGameTest.Units;

namespace TacticsGameTest
{
    internal class MapManagement
    {
        static MapManagement _instance;

        private Dictionary<Vec2Int, (Level, TileObject)> rooms = new Dictionary<Vec2Int, (Level, TileObject)>()
            {
                {new Vec2Int(2,8), (new RoomIntro(), null) },
                {new Vec2Int(6,7), (new RoomTower(), null) },
                {new Vec2Int(2,1), (new RoomTower(), null) }, //TODO: Change this level
                {new Vec2Int(10,8), (new RoomMine(), null)},
                {new Vec2Int(5,5), (new RoomAmbush(), null)},
                {new Vec2Int(5,0), (new RoomBoss(), null) } // Boss Room
            };
        private Dictionary<Vec2Int, TileObject> heals = new Dictionary<Vec2Int, TileObject>
            {
                {new Vec2Int(4,8), null },
                {new Vec2Int(7,4), null },
                {new Vec2Int(2,4), null }
            };
        private Dictionary<Vec2Int, TileObject> unlockers = new Dictionary<Vec2Int, TileObject>
            {
                {new Vec2Int(11,8), null },
                {new Vec2Int(3,3), null }
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
            party.SetPosition(new Vec2Int(0, 6));



            OverworldScenario = new MapScenario();

            var testControlGroup = new MapControlGroup();

            OverworldScenario.AddControlGroup(testControlGroup);

            testControlGroup.AddActor(party);
            OverworldScenario.BeginScenario();
            SetUpSprites();
        }

        private void SetUpSprites()
        {
            foreach (var item in unlockers.Keys)
            {
                unlockers[item] = new TileObject();
                unlockers[item].AddToGrid(OverworldMap.grid, 2);
                unlockers[item].SetPosition(item, false);
                unlockers[item].SetSpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("key.png"));
            }
            foreach (var item in heals.Keys)
            {
                heals[item] = new TileObject();
                heals[item].AddToGrid(OverworldMap.grid, 2);
                heals[item].SetPosition(item, false);
                heals[item].SetSpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("health.png"));
            }
            foreach (var item in rooms.Keys)
            {
                var room_sprite = new TileObject();
                rooms[item] = (rooms[item].Item1, room_sprite);
                room_sprite.AddToGrid(OverworldMap.grid, 2);
                room_sprite.SetPosition(item, false);
                if (item.x == 5 && item.y == 0)
                {
                    room_sprite.SetSpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("door_closed.png"));
                }
                else room_sprite.SetSpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("room_marker.png"));
            }

        }
        public void LoadOverworld()
        {
            if (OverworldMap == null)
            {
                SetupOverworld();
            }
            Bootstrap.GetCameraSystem().Size = 4 * 24;
            Bootstrap.GetCameraSystem().Position = new Vector2(85, 75);
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
            if (rooms.TryGetValue(pos, out var values))
            {
                if (CheckBossRoom(pos)) return;
                LoadRoom(values.Item1);
            }
            if (heals.ContainsKey(pos))
            {
                PlayerCharacterData.TankPlayer().stats.Hp = PlayerCharacterData.TankPlayer().stats.MaxHp;
                PlayerCharacterData.RoguePlayer().stats.Hp = PlayerCharacterData.RoguePlayer().stats.MaxHp;
                PlayerCharacterData.SpearPlayer().stats.Hp = PlayerCharacterData.SpearPlayer().stats.MaxHp;

                Audio.I.PlayAudio("ConfirmJingle");
                heals[pos].RemoveFromGrid();
                heals.Remove(pos);
            }
            if (unlockers.ContainsKey(pos))
            {
                PlayerCharacterData.keys += 1;
                if(PlayerCharacterData.keys >= 2) {
                    // Set door sprite to open
                    rooms[new Vec2Int(5,0)].Item2.SetSpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("door_open.png"));
                }
                unlockers[pos].RemoveFromGrid();
                unlockers.Remove(pos);

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
                    PlayerCharacterData.entered_boss = true; // Boss is true
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
