using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest.Combat
{
    internal class PlayerControlGroup : ControlGroup
    {
        public string Name { get; private set; }
        public PlayerControlGroup(string name)
        {
            Name = name;
        }
        public override float CalculateInitiative()
        {
            return 1;
        }

        public override void OnEndRound()
        {
        }

        public override void OnEndTurn()
        {
            foreach (var act in GetActors())
            {
                act.Graphic.Modulation = Color.White;
            }
        }

        public override void OnStartRound()
        {
        }

        public override void OnStartTurn()
        {
        }
    }
}
