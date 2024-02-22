using Kintsugi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debugging
{
    internal class Debugging : Game
    {
        public override void Initialize()
        {
            Dice.Roll(5, 20, true);
            Dice.Roll(5, 1, false, true);
        }

        public override void Update()
        {
        
        }
    }
}
