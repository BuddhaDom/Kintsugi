using Kintsugi.Objects;

namespace TacticsGameTest
{
    internal class MovingScenario : ScenarioManager
    {
        public override void OnBeginRound()
        {
        }

        public override void OnBeginScenario()
        {
            RecalculateInitiativeOnNewRound = true;
            Console.WriteLine("Scenario begun!");
        }

        public override void OnBeginTurn()
        {
        }

        public override void OnEndRound()
        {
        }

        public override void OnEndScenario()
        {
            Console.WriteLine("Scenario end!");
        }

        public override void OnEndTurn()
        {
        }
    }

}
