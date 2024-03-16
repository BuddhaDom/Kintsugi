using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.Input;
using Kintsugi.Objects;
using SDL2;

namespace PuzzleGame
{
    internal class BaseActor : Actor
    {
        public override void OnEndRound()
        {
        }

        public override void OnEndTurn()
        {
        }

        public override void OnStartRound()
        {
        }

        public override void OnStartTurn()
        {
        }
    }
    internal class MovementActor : BaseActor, IInputListener
    {
        private string name;
        public int speed;
        public int reverse_movement = 1;
        public MovementActor(string name)
        {
            this.name = name;
            LevelManager.Instance.AddLevelListener(this);
        }
        private static MoveEvent moveEvent;
        public void HandleInput(InputEvent inp, string eventType)
        {
            if (InTurn)
            {
                if (eventType == "KeyDown")
                {

                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                    {
                        QueueMove(Vec2Int.Down * reverse_movement);
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                    {
                        QueueMove(Vec2Int.Up * reverse_movement);
                        EndTurn();

                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        QueueMove(Vec2Int.Left * reverse_movement);
                        EndTurn();

                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        QueueMove(Vec2Int.Right * reverse_movement);
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        EndTurn();
                    }
                }
            }
        }

        public void QueueMove(Vec2Int move)
        {
            if (moveEvent == null)
            {
                moveEvent = new(Transform.Grid);
                EventManager.I.Queue(moveEvent);
            }
            moveEvent.AddActorToMove(this, move, speed);
        }

        public override void OnEndRound()
        {
            //Console.WriteLine(name + " End Round");
            moveEvent = null;
        }

        public override void OnEndTurn()
        {
            //Console.WriteLine(name + " End Turn");
        }

        public override void OnStartRound()
        {
        }

        public override void OnStartTurn()
        {

            //Console.WriteLine(name + " Start Turn");
        }
    }
}
