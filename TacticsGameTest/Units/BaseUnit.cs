using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest.Units
{
    internal class BaseUnit :
        Actor
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }

        public int Swift { get; set; } = 1;

        public int Intuition { get; set; } = 1;

        public int Brawn { get; set; } = 1;

        public int DamageMelee { get; set; }

        public int DamageRanged { get; set; }


        public override void OnEndRound()
        {
            throw new NotImplementedException();
        }

        public override void OnEndTurn()
        {
            throw new NotImplementedException();
        }

        public override void OnStartRound()
        {
            throw new NotImplementedException();
        }

        public override void OnStartTurn()
        {
            throw new NotImplementedException();
        }
    }
}
