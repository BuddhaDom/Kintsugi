using Kintsugi.Objects;

namespace TacticsGameTest
{
    internal class UnitControlGroup : ControlGroup
    {
        private string name;
        public UnitControlGroup(string name)
        {
            this.name = name;
        }
        public override float CalculateInitiative()
        {
            return 1;
        }

        public override void OnEndRound()
        {
            //Console.WriteLine("CG End Round");
            CurrentInitiative = 0;
        }

        public override void OnEndTurn()
        {
            //Console.WriteLine("CG End Turn");
        }

        public override void OnStartRound()
        {
            //Console.WriteLine("CG Start Round");
        }

        public override void OnStartTurn()
        {
            //Console.WriteLine("CG Start Turn");
        }

    }
}
