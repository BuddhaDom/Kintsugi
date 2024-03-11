using SDL2;
using System.Drawing;
using System.Numerics;

namespace Kintsugi.Objects.Graphics;

/// <summary>
/// A single image graphic for for <see cref="TileObject"/> 
/// </summary>
public class SpriteSingle : ISpriteable
{
    /// <summary>
    /// Image containing the sprite relevant to this graphic.
    /// </summary>
    public Sprite Sprite { get; internal set; }
    public ISpriteProperties Properties => Sprite;
    public SDL.SDL_Rect SourceRect()
        => new() {
            x = 0,
            y = 0,
            w = Sprite.Dimensions.x,
            h = Sprite.Dimensions.y
        };

    public bool Flipped { get; set; }
    public Color Modulation { get; set; } = Color.White;
    public Vector2 Scale { get; set; } = Vector2.One;
}