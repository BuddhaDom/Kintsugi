using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Tiles;
using System.Drawing;
using System.Numerics;
using Kintsugi.EventSystem;
using TacticsGameTest.UI;
using TacticsGameTest.Units;

namespace TacticsGameTest
{
    internal class GridBase : Grid, IInputListener
    {
        public GridBase(string tmxPath, bool gridVisible = false, Color gridColor = default) : base(tmxPath, gridVisible, gridColor)
        {
        }

        public GridBase(int gridWidth, int gridHeight, int tileWidth, string[] tileSetPaths, GridLayer[]? layers = null, bool gridVisible = false, Color gridColor = default) : base(gridWidth, gridHeight, tileWidth, tileSetPaths, layers, gridVisible, gridColor)
        {
        }
        private PlayerActor selectedActor;
        public void HandleInput(InputEvent inp, string eventType)
        {
            if (eventType == "MouseMotion" && HUD.Instance.Hovered is null)
            {
                var gridPos = WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                CursorTileObject.Cursor.SetCursor(this, gridPos, 4);
            }
            if (eventType == "MouseDown" && HUD.Instance.Hovered is null)
            {
                var gridPos = WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));

                var objects = GetObjectsAtPosition(gridPos);
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
        }
    }
}
