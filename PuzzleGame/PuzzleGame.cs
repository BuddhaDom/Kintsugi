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
using PuzzleGame.Levels;
using Kintsugi.Audio;

namespace PuzzleGame
{
    internal class PuzzleGame : Game, IInputListener
    {
        private Grid grid;
        private MovementActor character;
        private MovementActor character2;
        private MovingScenario scenario;

        public override void Initialize()
        {
            var master_bank = ((SoundFMOD)Bootstrap.GetSound()).LoadBank(Bootstrap.GetAssetManager().GetAssetPath("audio\\fmod_project\\Build\\Desktop\\Master.bank"));
            ((SoundFMOD)Bootstrap.GetSound()).LoadBank(Bootstrap.GetAssetManager().GetAssetPath("audio\\fmod_project\\Build\\Desktop\\Master.strings.bank"));
            master_bank.PreloadSamples();
            var ambience = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/DungeonAmbience");
            ambience.PlayImmediate();

            LevelManager.Instance.LoadNext();

            Bootstrap.GetInput().AddListener(this);
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
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_UP)
                {
                    up = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_DOWN)
                {
                    down = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT)
                {
                    right = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT)
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
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_UP)
                {
                    up = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_DOWN)
                {
                    down = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT)
                {
                    right = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT)
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
