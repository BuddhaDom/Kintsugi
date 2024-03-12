using System.Drawing;
using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Objects.Graphics;
using SDL2;

namespace Kintsugi.UI;

/// <summary>
/// An object that can be included in a canvas.
/// </summary>
public class CanvasObject: GraphicsObject
{
    /// <summary>
    /// Text to display.
    /// </summary>
    public string Text { get; set; } = "";
    /// <summary>
    /// Color of text to display.
    /// </summary>
    public Color TextColor { get; set; } = Color.LightGray;
    /// <summary>
    /// Position of the text within its object.
    /// </summary>
    public Vector2 TextPosition { get; set; }
    /// <summary>
    /// Size of the font to display.
    /// </summary>
    public int FontSize { get; set; }
    /// <summary>
    /// Path of the font to display.
    /// </summary>
    public string FontPath { get; set; } = "";
    /// <summary>
    /// Position of the CanvasObject in screen space.
    /// </summary>
    public Vector2 Position { get; set; }
    /// <summary>
    /// Pivot that object is placed on, (0,0) is top right and (1,1) is bottom left.
    /// If not following tileobject, this is the pivot of the object relative to the screen.
    /// If following tileobject, this is the pivot of the object relative to the tileobject.
    /// </summary>
    public Vector2 TargetPivot { get; set; }
    /// <summary>
    /// Tileobject that this canvas object follows.
    /// If null, it is positioned on screen space.
    /// If set, the canvas objects position will be relative to the tile object.
    /// </summary>
    public TileObject? FollowedTileobject { get; set; }
}