using Kintsugi.Objects;

namespace TacticsGameTest
{
    internal class MovingScenario : ScenarioManager
    {
        public override void OnBeginScenario()
        {
            Console.WriteLine("Scenario begun!");
        }

        public override void OnEndScenario()
        {
            Console.WriteLine("Scenario end!");
        }
    }
    internal class MyControlGroup : ControlGroup
    {
        public override float CalculateInitiative()
        {
            return 1;
        }

        public override void OnEndRound()
        {
            Console.WriteLine("CG End Round");
        }

        public override void OnEndTurn()
        {
            Console.WriteLine("CG End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine("CG Start Round");
        }

        public override void OnStartTurn()
        {
            Console.WriteLine("CG Start Turn");
        }
    }

}
