using Kintsugi.Core;
using System.Numerics;
using TacticsGameTest.Map;
using TacticsGameTest.Rooms;

namespace TacticsGameTest
{
    internal class MapManagement
    {
        static MapManagement _instance;
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
        public void EnterRoom(Vec2Int pos)
        {
            LoadRoom(new Room2());
        }
        private void LoadRoom(Level level)
        {
            currentRoom = level;
            UnloadOverworld();
            level.Load();
        }
    }
}
