using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Objects.Graphics;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Units;

namespace TacticsGameTest
{
    static class ActorFactory
    {

        public static SelectableActor Grunt(Grid grid)
        {
            var unit = new SelectableActor("grunt", "FantasyBattlePack\\SwordFighter\\Longhair\\Blue1.png");
            unit.MaxHp = 10;
            unit.AddToGrid(grid, 2);

            return unit;

        }

    }
}
