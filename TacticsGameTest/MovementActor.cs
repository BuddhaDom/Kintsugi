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
                        var event1 = new ActionEvent(() => Move(Vec2Int.Down))
                            .AddFinishAwait(this.Easing);

                        var event2 = new ActionEvent(() => Move(Vec2Int.Down))
                            .AddStartAwait(event1)
                            .AddFinishAwait(this.Easing)
                            .SetAsQueueBlocker();

                        EventManager.I.Queue(event1);
                        EventManager.I.Queue(event2);
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                    {
                        var event1 = new ActionEvent(() => Move(Vec2Int.Up))
                            .AddFinishAwait(this.Easing);

                        var event2 = new ActionEvent(() => Move(Vec2Int.Up))
                            .AddStartAwait(event1)
                            .AddFinishAwait(this.Easing)
                            .SetAsQueueBlocker();

                        EventManager.I.Queue(event1);
                        EventManager.I.Queue(event2);
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        var event1 = new ActionEvent(() => Move(Vec2Int.Left))
                            .AddFinishAwait(this.Easing);

                        var event2 = new ActionEvent(() => Move(Vec2Int.Left))
                            .AddStartAwait(event1)
                            .AddFinishAwait(this.Easing)
                            .SetAsQueueBlocker();

                        EventManager.I.Queue(event1);
                        EventManager.I.Queue(event2);
                        if (Graphic != null) Graphic.Flipped = true;
                        EndTurn();
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        var event1 = new ActionEvent(() => Move(Vec2Int.Right))
                            .AddFinishAwait(this.Easing);

                        var event2 = new ActionEvent(() => Move(Vec2Int.Right))
                            .AddStartAwait(event1)
                            .AddFinishAwait(this.Easing)
                            .SetAsQueueBlocker();

                        EventManager.I.Queue(event1);
                        EventManager.I.Queue(event2);
                        if (Graphic != null) Graphic.Flipped = false;
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
