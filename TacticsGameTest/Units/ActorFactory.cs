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

        public static PlayerActor Player(Grid grid, PlayerCharacterData data)
        {
            var unit = new PlayerActor("playerchar", data);
            unit.AddToGrid(grid, 2);
            return unit;
        }


        public static CombatActor Grunt(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 2;
            stats.Intuition = 1;
            stats.Swift = 2;

            stats.DamageMeleeAmount = 1;
            stats.DamageMeleeType = 8;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 4;

            stats.MaxHp = 15;

            var unit = new BasicMeleeEnemy("grunt", "FantasyBattlePack\\SwordFighter\\Longhair\\Red1.png", stats);
            unit.AddToGrid(grid, 2);

            return unit;

        }
        public static CombatActor Archer(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 1;
            stats.Intuition = 1;
            stats.Swift = 2;

            stats.DamageMeleeAmount = 1;
            stats.DamageMeleeType = 4;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 6;

            stats.MaxHp = 10;
            var unit = new BasicMeleeEnemy("archer", "FantasyBattlePack\\Archer\\Red1.png", stats);
            unit.AddToGrid(grid, 2);

            return unit;

        }

        public static CombatActor Ninja(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 1;
            stats.Intuition = 1;
            stats.Swift = 5;

            stats.MaxHp = 16;

            stats.DamageMeleeAmount = 1;
            stats.DamageMeleeType = 10;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 8;


            var unit = new BasicMeleeEnemy("ninja", "FantasyBattlePack\\Thief\\Red1.png", stats);
            unit.AddToGrid(grid, 2);

            return unit;

        }

        public static CombatActor Executioner(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 5;
            stats.Intuition = 2;
            stats.Swift = 1;

            stats.MaxHp = 24;

            stats.DamageMeleeAmount = 3;
            stats.DamageMeleeType = 12;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 4;

            var unit = new BasicMeleeEnemy("executioner", "FantasyBattlePack\\AxeFighter\\ShortHair\\Red2.png", stats);

            unit.AddToGrid(grid, 2);

            return unit;

        }


    }
}
