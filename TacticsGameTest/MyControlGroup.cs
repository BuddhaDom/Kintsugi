using Kintsugi.Core;
using Kintsugi.Objects;
using System.Drawing;

namespace TacticsGameTest
{
    internal class MyControlGroup : ControlGroup
    {
        private string name;
        public float Initiative;
        public MyControlGroup(string name)
        {
            this.name = name;
        }
        public override float CalculateInitiative()
        {
            // we not rolling for now
            //var initiative = Dice.Roll(1, 20);
            //Console.WriteLine(name + " rolled initiative " + initiative);
            return Initiative;
        }

        public override void OnEndRound()
        {
            Console.WriteLine("CG End Round");
            CurrentInitiative = 0;
        }

        public override void OnEndTurn()
        {
            foreach (var act in GetActors())
            {
                act.Graphic.Modulation = Color.White;
            }
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
