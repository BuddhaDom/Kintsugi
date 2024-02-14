using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Objects;
using Kintsugi_Engine.Objects;
using SDL2;

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
