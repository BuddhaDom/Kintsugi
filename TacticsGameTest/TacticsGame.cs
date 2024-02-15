using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Objects;
using Kintsugi.Rendering;
using Kintsugi.Tiles;
using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest
{
    internal class TacticsGame : Game, IInputListener
    {
        private GameObject grid;

        private MovingScenario scenario;

        public override void Initialize()
        {
            grid = new Grid(GetAssetManager().GetAssetPath("TiledTesting\\forestpath.tmx"), gridVisible: false, gridColor: Color.DarkBlue);
            grid.Transform.X = 0;
            grid.Transform.Y = 0;
            Bootstrap.GetCameraSystem().Size = 16 * 10;

            scenario = new MovingScenario();
            var group = new MyControlGroup();
            var actor = new MovementActor("actor1");
            var actor2 = new MovementActor("actor2");

            group.AddActor(actor);
            group.AddActor(actor2);

            var group2 = new MyControlGroup();
            var actor3 = new MovementActor("actor3");

            group2.AddActor(actor3);

            scenario.AddControlGroup(group);
            scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }

        public override void Update()
        {

            //Bootstrap.GetDisplay().ShowText("FPS: " + Bootstrap.GetSecondFPS() + " / " + Bootstrap.GetFPS(), 10, 10, 12, 255, 255, 255);

            var movement = Vector2.Zero;
            if (up)
            {
                movement = movement + new Vector2(0, -(float)Bootstrap.GetDeltaTime());
            }
            if (down)
            {
                movement = movement + new Vector2(0, (float)Bootstrap.GetDeltaTime());
            }
            if (left)
            {
                movement = movement + new Vector2(-(float)Bootstrap.GetDeltaTime(), 0);
            }
            if (right)
            {
                movement = movement + new Vector2((float)Bootstrap.GetDeltaTime(), 0);
            }
            Bootstrap.GetCameraSystem().Position += movement * 1f * Bootstrap.GetCameraSystem().Size;

            if (zoomOut)
            {
                Bootstrap.GetCameraSystem().Size *= 1f + 1f * (float)Bootstrap.GetDeltaTime();
            }
            if (zoomIn)
            {
                Bootstrap.GetCameraSystem().Size *= (1 / (1f + 1f * (float)Bootstrap.GetDeltaTime()));
            }
        }

        bool up, down, left, right, zoomIn, zoomOut;
        public void HandleInput(InputEvent inp, string eventType)
        {
            if (eventType == "KeyDown")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = true;
                }
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_Q)
                {
                    zoomIn = true;
                }
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_E)
                {
                    zoomOut = true;
                }


            }
            else if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = false;
                }
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_Q)
                {
                    zoomIn = false;
                }
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_E)
                {
                    zoomOut = false;
                }


            }


        }

    }
}
