using Kintsugi.Objects;
using TacticsGameTest.Units;

namespace TacticsGameTest.Combat
{
    internal class CombatScenario : ScenarioManager
    {
        public List<CombatActor> players = new();
        public List<CombatActor> enemies = new();
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
            
        }

        public override void OnEndScenario()
        {

        }

        public override void OnEndTurn()
        {
            if (players.All((p) => p.Dead))
            {
                EndScenario();
                Console.WriteLine("you lost :(");

            }
            else if (enemies.All((e) => e.Dead))
            {
                EndScenario();
                MapManagement.I.LoadOverworld();
            }
        }
    }
}
