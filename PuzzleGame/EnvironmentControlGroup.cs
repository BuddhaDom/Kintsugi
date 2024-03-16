using Kintsugi.Objects;

namespace PuzzleGame
{
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
