using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Tiles;
using SDL2;
using System.Drawing;
using System.Numerics;
using Engine.EventSystem;
using Kintsugi.Objects.Graphics;
using Kintsugi.UI;
using Kintsugi.Rendering;
using System.Diagnostics.Tracing;
using static System.Net.Mime.MediaTypeNames;
using TacticsGameTest.UI;
using TacticsGameTest.Units;
using TacticsGameTest.Combat;
using TacticsGameTest.Rooms;
using Kintsugi.Audio;
using TacticsGameTest.Map;

namespace TacticsGameTest
{
    internal class TacticsGame : Game, IInputListener
    {
        private Grid grid;
        private CombatScenario scenario;

        public override void Initialize()
        {
            // Audio
            var master_bank = ((SoundFMOD)Bootstrap.GetSound()).LoadBank(Bootstrap.GetAssetManager().GetAssetPath("fmod_project\\Build\\Desktop\\Master.bank"));
            ((SoundFMOD)Bootstrap.GetSound()).LoadBank(Bootstrap.GetAssetManager().GetAssetPath("fmod_project\\Build\\Desktop\\Master.strings.bank"));
            master_bank.PreloadSamples();



            Audio.I.bgfx.Start();
            //music.Start();


            /*
            // Levels
            var testlevel = new Room1();
            testlevel.Load();
            grid = testlevel.grid;
            scenario.AddControlGroup(group);
            scenario.AddControlGroup(group2);

            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
            */



            //var testlevel = new Room1();
            //testlevel.Load();
            //grid = testlevel.grid;

            MapManagement.I.LoadOverworld();

            Bootstrap.GetInput().AddListener(this);
        }

        
        private void CameraMovement()
        {
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
        public override void Update()
        {
            CameraMovement();
            //Bootstrap.GetDisplay().ShowText("FPS: " + Bootstrap.GetSecondFPS() + " / " + Bootstrap.GetFPS(), 10, 10, 12, 255, 255, 255);



        }

        private PlayerActor selectedActor;
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
