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


        // Amount is the 1 in 1d6
        private int damagemeleeamount;
        public int DamageMeleeAmount
        {
            get { return damagemeleeamount; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Only positive values are allowed");
                damagemeleeamount = value;
            }
        }

        private int damagerangedamount;
        public int DamageRangedAmount
        {
            get { return damagerangedamount; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Only positive values are allowed");
                damagerangedamount = value;
            }
        }

        // Type is the 6 in 1d6
        private int damagemeleetype;
        public int DamageMeleeType
        {
            get { return damagemeleetype; }
            set {
                if (value < 0) 
                    throw new ArgumentOutOfRangeException("Only positive values are allowed");
                damagemeleetype = value; }
        }

        private int damagerangedtype;
        public int DamageRangedType
        {
            get { return damagerangedtype; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Only positive values are allowed");
                damagerangedtype = value;
            }
        }



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
