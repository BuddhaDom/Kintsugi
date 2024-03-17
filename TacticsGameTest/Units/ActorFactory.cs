using Kintsugi.Tiles;
using TacticsGameTest.Units;

namespace TacticsGameTest
{
    static class ActorFactory
    {

        public static CombatActor PlayerCharacter(Grid grid)
        {
            var unit = new PlayerActor("player", "FantasyBattlePack\\SwordFighter\\Longhair\\Blue1.png");

            unit.Brawn = 2;
            unit.Intuition = 1;
            unit.Swift = 2;

            unit.DamageMeleeAmount = 1;
            unit.DamageMeleeType = 8;

            unit.DamageRangedAmount = 1;
            unit.DamageRangedType = 4;

            unit.MaxHp = 15;
            unit.Hp = unit.MaxHp;
            unit.AddToGrid(grid, 2);

            return unit;
        }
        public static CombatActor Grunt(Grid grid)
        {
            var unit = new BasicMeleeEnemy("grunt", "FantasyBattlePack\\SwordFighter\\Longhair\\Red1.png");

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
        public static CombatActor Archer(Grid grid)
        {
            var unit = new BasicMeleeEnemy("archer", "FantasyBattlePack\\Archer\\Red1.png");
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

        public static CombatActor Ninja(Grid grid)
        {
            var unit = new BasicMeleeEnemy("ninja", "FantasyBattlePack\\Thief\\Red1.png");
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

        public static CombatActor Executioner(Grid grid)
        {
            var unit = new BasicMeleeEnemy("executioner", "FantasyBattlePack\\AxeFighter\\ShortHair\\Red2.png");
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
