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
            Dice.Roll(3, 6, false); // 3d6
            Dice.Roll(5, 1, true);
        }

        public override void Update()
        {
        
        }
    }
}
