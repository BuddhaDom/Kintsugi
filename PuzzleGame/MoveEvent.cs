using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.Tiles;

namespace PuzzleGame
{
    internal class ActorMove
    {
        internal Vec2Int move;
        internal int times;
    }
    internal class MoveEvent : Event
    {
        private Grid grid;
        public MoveEvent(Grid grid)
        {
            this.grid = grid;
        }
        public void AddActorToMove(MovementActor actor, Vec2Int move, int times)
        {
            movementDictionary.Add(actor, new ActorMove { move = move, times = times });
        }
        private Dictionary<MovementActor, ActorMove> movementDictionary = new();
        public override void OnExecute()
        {
            while (movementDictionary.Count > 0)
            {
                MoveActor(movementDictionary.First().Key);
            }
        }
        private void MoveActor(MovementActor movementActor)
        {
            if (!movementDictionary.ContainsKey(movementActor) || movementDictionary[movementActor].times <= 0)
            {
                return;
            }

            Vec2Int targetPosition = movementActor.Transform.Position + movementDictionary[movementActor].move;

            if (!CollisionSystem.CollidesColliderWithPosition(movementActor.Collider, movementActor.Transform.Grid, movementActor.Transform.Position + movementDictionary[movementActor].move))
            {
                movementActor.Move(movementDictionary[movementActor].move);
                movementDictionary[movementActor].times--;
                if (movementDictionary[movementActor].times <= 0)
                {
                    movementDictionary.Remove(movementActor);
                }
            }
            else
            {
                var targetObjects = grid.GetObjectsAtPosition(targetPosition);
                if (targetObjects == null)
                {
                    movementDictionary.Remove(movementActor);
                    return;
                }
                List<MovementActor> newObjects = new List<MovementActor>();
                foreach (var targetObject in targetObjects)
                {
                    if (targetObject is MovementActor act)
                    {
                        if (movementDictionary.ContainsKey(act) && movementDictionary[act].times > 0)
                        {
                            newObjects.Add(act);
                        }
                    }
                }
                if (newObjects.Count == 0)
                {
                    movementDictionary.Remove(movementActor);

                }
                else
                {
                    foreach (var act in newObjects)
                    {
                        MoveActor(act);
                    }
                }
            }

        }
    }
}
