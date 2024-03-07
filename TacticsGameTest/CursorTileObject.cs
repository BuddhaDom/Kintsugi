using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TweenSharp.Animation;

namespace TacticsGameTest
{
    internal class CursorTileObject : TileObject
    {
        private CursorTileObject()
        {
        }
        private static void Initialize()
        {
            var cursor = new CursorTileObject();
            var frames = new List<int>();
            frames.AddRange(Enumerable.Range(00, 16));
            frames.AddRange(Enumerable.Range(20, 16));
            frames.AddRange(Enumerable.Range(40, 16));

            cursor.SetSpriteSingle(
                Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath("TinyBattles\\cursor.png")
            );

            cursor.SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.1);
            _cursor = cursor;
        }
        private static CursorTileObject _cursor;
        public static CursorTileObject Cursor { get {
                if (_cursor == null)
                {
                    Initialize();
                }
                return _cursor;
            } }
        public void SetCursor(Grid grid, Vec2Int position, int layer)
        {
            if (grid != Transform.Grid)
            {
                Cursor.AddToGrid(grid, layer);

            }
            Cursor.SetPosition(position);
        }
    }
}
