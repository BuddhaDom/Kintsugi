using Kintsugi.Audio;
using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.Objects;

namespace PuzzleGame
{
    internal class MovingScenario : ScenarioManager
    {
        public List<Actor> goals = new();
        public override void OnBeginRound()
        {
        }

        public override void OnBeginScenario()
        {
            RecalculateInitiativeOnNewRound = true;
            Console.WriteLine("Scenario begun!");
        }

        public override void OnBeginTurn()
        {
        }

        public override void OnEndRound()
        {

            if (goals.Count > 0)
            {
                var allCollides = true;

                foreach (var goal in goals)
                {
                    if (!CollisionSystem.CollidesColliderWithPosition(goal.Collider, goal.Transform.Grid, goal.Transform.Position))
                    {
                        allCollides = false;
                    }
                }

                if (allCollides)
                {

                    Console.WriteLine("cogneratiualations");
                    var sfx_success = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/ConfirmJingle");
                    sfx_success.PlayImmediate();
                    LevelManager.Instance.LoadNext();
                }
            }
        }

        public override void OnEndScenario()
        {
            Console.WriteLine("Scenario end!");
        }

        public override void OnEndTurn()
        {
        }
    }
}
