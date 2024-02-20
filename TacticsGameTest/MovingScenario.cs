using Kintsugi.Core;
using Kintsugi.Objects;

namespace TacticsGameTest
{
    internal class MovingScenario : ScenarioManager
    {
        public override void OnBeginScenario()
        {
            RecalculateInitiativeOnNewRound = true;
            Console.WriteLine("Scenario begun!");
        }

        public override void OnEndScenario()
        {
            Console.WriteLine("Scenario end!");
        }
    }
    internal class MyControlGroup : ControlGroup
    {
        private string name;
        public MyControlGroup(string name)
        {
            this.name = name;
        }
        public override float CalculateInitiative()
        {
            var initiative = Dice.Roll(1, 20);
            Console.WriteLine(name + " rolled initiative " + initiative);
            return initiative;
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
