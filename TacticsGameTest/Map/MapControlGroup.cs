using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest.Map
{
    internal class MapControlGroup : ControlGroup
    {
        public override float CalculateInitiative()
        {
            return 0;
        }



        public override void OnEndRound()
        {
        }

        public override void OnEndTurn()
        {
        }

        public override void OnStartRound()
        {
        }

        public override void OnStartTurn()
        {
        }
    }
}
