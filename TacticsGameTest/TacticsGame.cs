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

namespace TacticsGameTest
{
    internal class TacticsGame : Game, IInputListener
    {
        private Grid grid;
        private CombatScenario scenario;

        public override void Initialize()
        {
            grid = new Grid(GetAssetManager().GetAssetPath("Tilemaps\\Levels\\TestLevel.tmx"))
            {
                Position =
                {
                    X = 0,
                    Y = 0
                }
            };
            Bootstrap.GetCameraSystem().Size = 16 * 10;
            var character = new PlayerActor("bro1", "FantasyBattlePack\\SwordFighter\\Longhair\\Blue1.png");
            character.AddToGrid(grid, 3);
            character.SetPosition(Vec2Int.One * 3);
            var character2 = new PlayerActor("bro2", "FantasyBattlePack\\Archer\\Blue1.png");
            character2.AddToGrid(grid, 3);
            character2.SetPosition(Vec2Int.One * 4);
            var character3 = new BasicMeleeEnemy("enemy1", "FantasyBattlePack\\AxeKnight\\Red.png");
            character3.AddToGrid(grid, 3);
            character3.SetPosition(Vec2Int.One * 6);

            // character.SetSpriteSingle(GetAssetManager().GetAssetPath("guy.png"), 
            //     Vector2.One / 2, new Vector2(6.5f, 8.5f));

            scenario = new CombatScenario();
            var group = new PlayerControlGroup("john's group");
            var group2 = new EnemyControlGroup("bob's group");

            group.AddActor(character);
            group.AddActor(character2);
            group2.AddActor(character3);
            //group.AddActor(character2);

            scenario.AddControlGroup(group);
            scenario.AddControlGroup(group2);

            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();

            
            //var testlevel = new Room1();
            //testlevel.Load();
            //grid = testlevel.grid;
           

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
            if (eventType == "MouseMotion")
            {
                var gridPos = grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                CursorTileObject.Cursor.SetCursor(grid, gridPos, 5);
            }
            if (eventType == "MouseDown")
            {
                var gridPos = grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));

                var objects = grid.GetObjectsAtPosition(gridPos);
                if (objects != null)
                {
                    PlayerActor selectableActor = null;
                    foreach (var item in objects)
                    {
                        if (item is PlayerActor a)
                        {
                            selectableActor = a;
                        }
                    }
                    if (selectableActor != null && selectableActor.InTurn && EventManager.I.IsQueueDone())
                    {
                        if (selectedActor != null)
                        {
                            selectedActor.Unselect();
                        }
                        selectedActor = selectableActor;
                        selectedActor.Select();
                        Console.WriteLine(selectableActor.name);
                    }

                }
                //Console.WriteLine(gridPos);
                //Console.WriteLine(grid.GridToWorldPosition(gridPos));

            }

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
