using Kintsugi.Objects;
using TacticsGameTest.Units;

namespace TacticsGameTest.Combat
{
    internal class CombatScenario : ScenarioManager
    {
        public List<CombatActor> players = new();
        public List<CombatActor> enemies = new();
        public override void OnBeginRound()
        {
        }

        public override void OnBeginScenario()
        {
            if (PlayerCharacterData.entered_boss) // Beat boss room
            {
                Audio.I.music.Stop();
                Audio.I.musicBoss.Start();
            }
        }

        public override void OnBeginTurn()
        {
        }

        public override void OnEndRound()
        {
            
        }

        public override void OnEndScenario()
        {

        }

        public override void OnEndTurn()
        {
            if (players.All((p) => p.Dead))
            {
                EndScenario();
                Audio.I.music.Stop();
                Console.WriteLine("you lost :(");

            }
            else if (enemies.All((e) => e.Dead))
            {
                EndScenario();
                if(PlayerCharacterData.entered_boss) // Beat boss room
                {
                    Audio.I.musicBoss.Stop();
                    Audio.I.PlayAudio("WinTune");
                }
                MapManagement.I.LoadOverworld();
            }
        }
    }
}
