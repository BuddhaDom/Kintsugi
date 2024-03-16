using System.Drawing;
using System.Numerics;
using Kintsugi.Objects;
using Kintsugi.Objects.Graphics;

namespace Kintsugi.UI;

/// <summary>
/// An object that can be included in a canvas.
/// </summary>
public class CanvasObject : GraphicsObject
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
    /// Pivot the text is drawn from.
    /// </summary>
    public Vector2 TextPivot { get; set; }

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

    /// <summary>
    /// Dimensions of the CanvasObject independent of its <see cref="GraphicsObject.Graphic"/>.
    /// </summary>
    public Vector2 Dimensions { get; set; }

    public bool Visible { get; set; }= true;

    public bool IsButton { get; set; } = false;

    public CanvasObject()
    {
        if (Graphic != null) Dimensions = Graphic.Properties.Dimensions;
    }

    /// <summary>
    /// Things to be done when this item is focused with mouse or other input.
    /// </summary>
    public virtual void OnHoverStart()
    {
        Console.WriteLine(this+" hovered");
    }
    
    /// <summary>
    /// Things to be done when this item is no longer focused with mouse or other input.
    /// </summary>
    public virtual void OnHoverEnd()
    {
        Console.WriteLine(this+" unhovered");

    }

    /// <summary>
    /// Things to be done when this item is clicked.
    /// </summary>
    public virtual void OnClick()
    {
        Console.WriteLine(this+" clicked");
    }

    public override void SetSpriteSingle(Sprite sprite)
    {
        base.SetSpriteSingle(sprite);
        if (Dimensions == Vector2.Zero) Dimensions = sprite.Dimensions;
    }

    public override void SetAnimation(SpriteSheet spriteSheet, double timeLength, IEnumerable<int> frames, int repeats = 0, bool bounces = false,
        bool autoStart = true)
    {
        base.SetAnimation(spriteSheet, timeLength, frames, repeats, bounces, autoStart);
        if (Dimensions == Vector2.Zero) Dimensions = spriteSheet.Dimensions;
    }
}