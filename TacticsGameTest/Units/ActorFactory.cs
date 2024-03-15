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
            var unit = new SelectableActor("grunt", "FantasyBattlePack\\SwordFighter\\Longhair\\Red1.png");

            unit.Brawn = 2;
            unit.Intuition = 1;
            unit.Swift = 2;

            unit.DamageMeleeAmount = 1;
            unit.DamageMeleeType = 8;

            unit.DamageRangedAmount = 1;
            unit.DamageRangedType = 4;

            unit.MaxHp = 15;
            unit.AddToGrid(grid, 2);

            return unit;

        }
        public static SelectableActor Archer(Grid grid)
        {
            var unit = new SelectableActor("archer", "FantasyBattlePack\\Archer\\Red1.png");
            unit.Brawn = 1;
            unit.Intuition = 1;
            unit.Swift = 2;

            unit.DamageMeleeAmount = 1;
            unit.DamageMeleeType = 4;

            unit.DamageRangedAmount = 1;
            unit.DamageRangedType = 6;

            unit.MaxHp = 10;
            unit.AddToGrid(grid, 2);

            return unit;

        }

        public static SelectableActor Ninja(Grid grid)
        {
            var unit = new SelectableActor("ninja", "FantasyBattlePack\\Thief\\Red1.png");
            unit.Brawn = 1;
            unit.Intuition = 1;
            unit.Swift = 5;

            unit.MaxHp = 16;

            unit.DamageMeleeAmount = 1;
            unit.DamageMeleeType = 10;

            unit.DamageRangedAmount = 1;
            unit.DamageRangedType = 8;


            unit.AddToGrid(grid, 2);

            return unit;

        }

        public static SelectableActor Executioner(Grid grid)
        {
            var unit = new SelectableActor("executioner", "FantasyBattlePack\\AxeFighter\\ShortHair\\Red2.png");
            unit.Brawn = 5;
            unit.Intuition = 2;
            unit.Swift = 1;

            unit.MaxHp = 24;

            unit.DamageMeleeAmount = 3;
            unit.DamageMeleeType = 12;

            unit.DamageRangedAmount = 1;
            unit.DamageRangedType = 4;


            unit.AddToGrid(grid, 2);

            return unit;

        }


    }
}
