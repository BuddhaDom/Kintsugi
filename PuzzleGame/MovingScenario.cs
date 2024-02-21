using Kintsugi.Core;
using Kintsugi.Objects;

namespace PuzzleGame
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
    internal class PlayerControlGroup : ControlGroup
    {
        private string name;
        public PlayerControlGroup(string name)
        {
            this.name = name;
        }
        public override float CalculateInitiative()
        {
            return 1;
        }

        public override void OnEndRound()
        {
            Console.WriteLine("CG End Round");
            CurrentInitiative = 0;
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
    internal class EnvironmentControlGroup : ControlGroup
    {
        private string name;
        public EnvironmentControlGroup(string name)
        {
            this.name = name;
        }
        public override float CalculateInitiative()
        {
            return 1;
        }

        public override void OnEndRound()
        {
            Console.WriteLine("CG End Round");
            CurrentInitiative = 0;
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
