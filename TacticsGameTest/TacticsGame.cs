using Kintsugi.Core;
using Kintsugi.Input;
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
using Kintsugi.Objects;

namespace TacticsGameTest
{
    internal class TacticsGame : Game, IInputListener
    {
        private Grid grid;
        private TileObject character;

        public override void Initialize()
        {
            grid = new Grid(GetAssetManager().GetAssetPath("TiledTesting\\forestpath.tmx"), gridVisible: false, gridColor: Color.DarkBlue);
            grid.Transform.X = 0;
            grid.Transform.Y = 0;
            Bootstrap.GetCameraSystem().Size = 16 * 10;

            
            // TODO: This sucks.
            var transform = new TileObjectTransform(Vec2Int.One * 3, 0, grid);
            var collider = new TileObjectCollider([0], [1]);
            var sprite = new TileObjectSprite(GetAssetManager().GetAssetPath("guy.png"), Vector2.One / 2,
                new Vector2(8, 8));
            character = new TileObject(transform,collider,sprite);
        }

        public override void Update()
        {
            Bootstrap.GetInput().AddListener(this);

            Bootstrap.GetDisplay().ShowText("FPS: " + Bootstrap.GetSecondFPS() + " / " + Bootstrap.GetFPS(), 10, 10, 12, 255, 255, 255);

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
