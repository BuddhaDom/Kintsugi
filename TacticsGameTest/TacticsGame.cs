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

namespace TacticsGameTest
{
    internal class TacticsGame : Game, IInputListener
    {
        private Grid grid;
        private MovingScenario scenario;

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
            var character = new SelectableActor("bro", "FantasyBattlePack\\SwordFighter\\Longhair\\Blue1.png");
            character.AddToGrid(grid, 3);
            character.SetPosition(Vec2Int.One * 3);
            var character2 = new SelectableActor("bro", "FantasyBattlePack\\Archer\\Blue1.png");
            character2.AddToGrid(grid, 3);
            character2.SetPosition(Vec2Int.One * 4);
            var character3 = new SelectableActor("bro", "FantasyBattlePack\\AxeKnight\\Red.png");
            character3.AddToGrid(grid, 3);
            character3.SetPosition(Vec2Int.One * 6);

            // character.SetSpriteSingle(GetAssetManager().GetAssetPath("guy.png"), 
            //     Vector2.One / 2, new Vector2(6.5f, 8.5f));

            scenario = new MovingScenario();
            var group = new MyControlGroup("john's group");
            var group2 = new MyControlGroup("bob's group");
            group2.Initiative = -2;

            group.AddActor(character);
            group.AddActor(character2);
            group2.AddActor(character3);
            //group.AddActor(character2);

            scenario.AddControlGroup(group);
            scenario.AddControlGroup(group2);

            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();

            /*
            var canvas = new Canvas();
            var canvasObject = new CanvasObject
            {
                FontPath = "Fonts\\calibri.ttf",
                Text = "Test",
                FontSize = 72,
                Position = Vector2.Zero
            };


            float scale = 4;
            IntPtr texture = ((DisplaySDL)Bootstrap.GetDisplay()).LoadTexture(GetAssetManager().GetAssetPath("guy.png"));

            for (int i = -2; i < 3; i++)
            {
                var obj1 = new CanvasObject();
                obj1.SetSpriteSingle(GetAssetManager().GetAssetPath("guy.png"), default, new Vector2(13/2f, 17/2f));
                obj1.Position = new Vector2(i * obj1.Graphic.Properties.Dimensions.x * scale, 0);
                obj1.Graphic.Scale = Vector2.One * scale;
                canvas.Objects.Add(obj1);
                obj1.FollowedTileobject = character;
                obj1.TargetPivot = new Vector2(0.25f, -0.25f);
                obj1.TextPivot = new Vector2(0.5f, 0.5f);

                obj1.Text = i.ToString();
                obj1.FontPath = "Fonts\\calibri.ttf";
                obj1.FontSize = 40;

            }

            canvasObject.SetSpriteSingle(GetAssetManager().GetAssetPath("guy.png"), default, new Vector2(8, 8));
            canvasObject.Graphic.Scale = new Vector2(1, 1);
            canvasObject.TargetPivot = new Vector2(0.5f, 0.5f);
            canvasObject.TextPivot = new Vector2(1f, 1f);

            //canvas.Position = Vector2.One * 100;
            canvas.Objects.Add(canvasObject);
            */

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

        private SelectableActor selectedActor;
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
                    SelectableActor selectableActor = null;
                    foreach (var item in objects)
                    {
                        if (item is SelectableActor a)
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
