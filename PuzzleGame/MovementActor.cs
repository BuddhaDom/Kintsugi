using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Objects;
using Kintsugi.EventSystem;
using SDL2;
using Engine.EventSystem;
using Kintsugi.EventSystem.Events;
using Kintsugi.Collision;
using Kintsugi.Objects.Properties;

namespace PuzzleGame
{
    internal class MovementActor : Actor, IInputListener
    {
        private string name;
        public int speed;
        public MovementActor(string name)
        {
            this.name = name;
            Bootstrap.GetInput().AddListener(this);
        }
        public void HandleInput(InputEvent inp, string eventType)
        {
            if (InTurn)
            {
                if (eventType == "KeyDown")
                {

                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                    {
                        QueueMove(Vec2Int.Down);
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                    {
                        QueueMove(Vec2Int.Up);
                        EndTurn();
                        
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        QueueMove(Vec2Int.Left);
                        EndTurn();
                        
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        QueueMove(Vec2Int.Right);
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
            for (int i = 0; i < speed; i++)
            {
                EventManager.I.Queue(
                                new ActionEvent(() => {
                                    if (!CollisionSystem.CollidesColliderWithPosition(Collider, Transform.Grid, Transform.Position + move))
                                        Move(move);
                                }
                                    ));
            }
        }

        public override void OnEndRound()
        {
            Console.WriteLine(name + " End Round");
        }

        public override void OnEndTurn()
        {
            Console.WriteLine(name + " End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine(name + " Start Round");
        }

        public override void OnStartTurn()
        {

            Console.WriteLine(name + " Start Turn");
        }
    }
}
