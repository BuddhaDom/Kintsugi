using Kintsugi.Objects.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Abilities;

namespace TacticsGameTest.Units
{
    internal class PlayerCharacterData
    {
        public CharacterStats stats;
        public string SpritePath;
        public List<Ability> abilities;


        static PlayerCharacterData _player1;
        public static PlayerCharacterData SpearPlayer()  // spear guy
        {
            if (_player1 == null)
            {
                _player1 = new PlayerCharacterData();
                _player1.SpritePath = "FantasyBattlePack\\SpearFighter\\LongHair\\Blue2.png";

                CharacterStats stats = new CharacterStats();
                stats.Brawn = 5;
                stats.Intuition = 2;
                stats.Swift = 1;

                stats.MaxHp = 5;
                stats.Hp = 5;

                stats.DamageMeleeAmount = 3;
                stats.DamageMeleeType = 12;

                stats.DamageRangedAmount = 1;
                stats.DamageRangedType = 4;
                _player1.stats = stats;
            }
            

            return _player1;
        }
        static PlayerCharacterData _player2;
        public static PlayerCharacterData TankPlayer() // tank guy
        {
            if (_player2 == null)
            {
                _player2 = new PlayerCharacterData();
                _player2.SpritePath = "FantasyBattlePack\\AxeKnight\\Blue.png";

                CharacterStats stats = new CharacterStats();
                stats.Brawn = 5;
                stats.Intuition = 2;
                stats.Swift = 1;

                stats.MaxHp = 5;
                stats.Hp = 5;

                stats.DamageMeleeAmount = 3;
                stats.DamageMeleeType = 12;

                stats.DamageRangedAmount = 1;
                stats.DamageRangedType = 4;
                _player2.stats = stats;
            }
            return _player2;
        }
        static PlayerCharacterData _player3;
        public static PlayerCharacterData RoguePlayer() // rogue guy
        {
            if (_player3 == null)
            {
                _player3 = new PlayerCharacterData();
                _player3.SpritePath = "FantasyBattlePack\\Thief\\Blue1.png";

                CharacterStats stats = new CharacterStats();
                stats.Brawn = 5;
                stats.Intuition = 2;
                stats.Swift = 1;

                stats.MaxHp = 5;
                stats.Hp = 5;

                stats.DamageMeleeAmount = 3;
                stats.DamageMeleeType = 12;

                stats.DamageRangedAmount = 1;
                stats.DamageRangedType = 4;
                _player3.stats = stats;
            }
            return _player3;
        }

    }
}
