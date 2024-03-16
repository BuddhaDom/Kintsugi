using Kintsugi.Objects;

namespace TacticsGameTest.Map
{
    internal class MapScenario : ScenarioManager
    {
        public int mapTimer = 0; // at 6, reset, bad thing happen?
        public override void OnBeginRound()
        {
        }

        public override void OnBeginScenario()
        {
        }

        public override void OnBeginTurn()
        {
        }

        public override void OnEndRound()
        {
            mapTimer += 1;
            if (mapTimer >= 6) {
                Console.WriteLine("1 hour passed");
                mapTimer = 0;
            }

        }

        public override void OnEndScenario()
        {
        }

        public override void OnEndTurn()
        {
        }
    }
}
