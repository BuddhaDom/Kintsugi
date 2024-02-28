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
    internal class PlayerControlGroup : ControlGroup
    {
        private string name;
        public PlayerControlGroup(string name)
        {
            this.name = name;
        }
        public override float CalculateInitiative()
        {
            return 1;
        }

        public override void OnEndRound()
        {
            Console.WriteLine("CG End Round");
            CurrentInitiative = 0;
        }

        public override void OnEndTurn()
        {
            Console.WriteLine("CG End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine("CG Start Round");
        }

        public override void OnStartTurn()
        {
            Console.WriteLine("CG Start Turn");
        }

    }
    internal class EnvironmentControlGroup : ControlGroup
    {
        private string name;
        public EnvironmentControlGroup(string name)
        {
            this.name = name;
        }
        public override float CalculateInitiative()
        {
            return 1;
        }

        public override void OnEndRound()
        {
            Console.WriteLine("CG End Round");
            CurrentInitiative = 0;
        }

        public override void OnEndTurn()
        {
            Console.WriteLine("CG End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine("CG Start Round");
        }

        public override void OnStartTurn()
        {
            Console.WriteLine("CG Start Turn");
        }

    }
}
