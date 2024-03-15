using Kintsugi.Objects;

namespace TacticsGameTest
{
    internal class EnemyControlGroup : UnitControlGroup
    {
        private string name;
        public EnemyControlGroup(string name): base(name)
        {
        }
        public override float CalculateInitiative()
        {
            return 0;
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
