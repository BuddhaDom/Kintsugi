﻿using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Tiles;
using System.Numerics;

namespace TacticsGameTest.UI
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

            cursor.SetAnimation(
                Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath("FantasyBattlePack\\SelectionCursor.png"),
                32,
                32,
                2,
                2f,
                Enumerable.Range(0, 2),
                new Vector2(-0.5f, -0.5f)
            );

            cursor.SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.1);
            _cursor = cursor;
        }
        private static CursorTileObject _cursor;
        public static CursorTileObject Cursor
        {
            get
            {
                if (_cursor == null)
                {
                    Initialize();
                }
                return _cursor;
            }
        }
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
