using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Objects;
using Kintsugi.EventSystem;
using SDL2;
using Engine.EventSystem;
using Kintsugi.EventSystem.Events;

namespace TacticsGameTest
{
    internal class MovementActor : Actor, IInputListener
    {
        private string name;
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
                        EventManager.I.Queue(
                            new ActionEvent(() =>
                                Move(Vec2Int.Up * -1) 
                                ));
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                    {
                        EventManager.I.Queue(
                            new ActionEvent(() =>
                                Move(Vec2Int.Down * -1) 
                                ));
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        EventManager.I.Queue(
                            new ActionEvent(() =>
                                Move(Vec2Int.Left) 
                                ));
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        EventManager.I.Queue(
                            new ActionEvent(() =>
                                Move(Vec2Int.Right) 
                                ));
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        EndTurn();
                    }
                }
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
