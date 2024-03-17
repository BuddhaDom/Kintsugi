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
using TacticsGameTest.Abilities;
using Kintsugi.Tiles;
using TacticsGameTest.Units;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TacticsGameTest
{
    static class ActorFactory
    {

        public static PlayerActor SpearPlayer(Grid grid)
        {
            var unit = new PlayerActor("Spear Player", PlayerCharacterData.SpearPlayer());
            unit.abilities = new();
            unit.abilities.Add(new Stride(unit));
            unit.abilities.Add(new SpearStab(unit));
            var basicBuffRange = new List<Vec2Int>() { };
            for (int x = -10; x < 10; x++)
            {
                for (int y = -10; y < 10; y++)
                {
                    basicBuffRange.Add(new Vec2Int(x, y));
                }
            }
            unit.abilities.Add(new BasicBuff(unit, basicBuffRange));
            unit.AddToGrid(grid, 5);
            return unit;
        }
        public static PlayerActor TankPlayer(Grid grid)
        {

            var unit = new PlayerActor("Tank Player", PlayerCharacterData.TankPlayer());
            unit.abilities = new();
            unit.abilities.Add(new Stride(unit));
            unit.abilities.Add(new AxeSwing(unit));
            var basicMeleeRange = new List<Vec2Int>()
            {
                new Vec2Int(-1, -1),
                new Vec2Int(-1, 0),
                new Vec2Int(-1, 1),
                new Vec2Int(0, -1),
                new Vec2Int(0, 1),
                new Vec2Int(1, -1),
                new Vec2Int(1, 0),
                new Vec2Int(1, 1),
            };

            unit.abilities.Add(new PushAttack(unit, basicMeleeRange));
            unit.abilities.Add(new Guard(unit));

            unit.AddToGrid(grid, 5);
            return unit;
        }
        public static PlayerActor RoguePlayer(Grid grid)
        {

            var unit = new PlayerActor("Rogue Player", PlayerCharacterData.RoguePlayer());
            unit.abilities = new();
            unit.abilities.Add(new Stride(unit));
            unit.abilities.Add(new PoisonDagger(unit));
            unit.abilities.Add(new DaggerThrow(unit));

            unit.AddToGrid(grid, 5);
            return unit;
        }


        public static CombatActor Grunt(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 2;
            stats.Intuition = 1;
            stats.Swift = 2;

            stats.DamageMeleeAmount = 1;
            stats.DamageMeleeType = 1;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 1;

            stats.MaxHp = 5;
            stats.Hp = 5;

            var unit = new BasicMeleeEnemy("grunt", "FantasyBattlePack\\SwordFighter\\LongHair\\Red1.png", stats);
            unit.AddToGrid(grid, 5);

            return unit;

        }
        public static CombatActor Archer(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 1;
            stats.Intuition = 1;
            stats.Swift = 2;

            stats.DamageMeleeAmount = 1;
            stats.DamageMeleeType = 1;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 1;

            stats.MaxHp = 5;
            stats.MaxHp = 5;
            var unit = new BasicRangedEnemy("archer", "FantasyBattlePack\\Archer\\Red1.png", stats);
            unit.AddToGrid(grid, 5);

            return unit;

        }

        public static CombatActor Ninja(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 1;
            stats.Intuition = 1;
            stats.Swift = 5;

            stats.MaxHp = 5;
            stats.MaxHp = 5;

            stats.DamageMeleeAmount = 1;
            stats.DamageMeleeType = 1;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 1;


            var unit = new BasicMeleeEnemy("ninja", "FantasyBattlePack\\Thief\\Red1.png", stats);
            unit.AddToGrid(grid, 5);

            return unit;

        }

        public static CombatActor Executioner(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 5;
            stats.Intuition = 2;
            stats.Swift = 1;

            stats.MaxHp = 5;
            stats.MaxHp = 5;

            stats.DamageMeleeAmount = 2;
            stats.DamageMeleeType = 6;

            stats.DamageRangedAmount = 1;
            stats.DamageRangedType = 1;

            var unit = new BasicMeleeEnemy("executioner", "FantasyBattlePack\\AxeFighter\\ShortHair\\Red2.png", stats);

            unit.AddToGrid(grid, 5);

            return unit;

        }

        public static CombatActor Necromancer(Grid grid)
        {
            var stats = new CharacterStats();
            stats.Brawn = 2;
            stats.Intuition = 2;
            stats.Swift = 2;

            stats.MaxHp = 16;
            stats.MaxHp = 16;

            stats.DamageMeleeAmount = 1;
            stats.DamageMeleeType = 1;

            stats.DamageRangedAmount = 2;
            stats.DamageRangedType = 1; // 2d1

            var unit = new BasicMeleeEnemy("necromancer", "FantasyBattlePack\\Wizard\\Red2.png", stats);

            unit.AddToGrid(grid, 5);

            return unit;

        }


    }
}
