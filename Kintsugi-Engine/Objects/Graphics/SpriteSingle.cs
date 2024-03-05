using SDL2;

namespace Kintsugi.Objects.Graphics;

public class SpriteSingle : ISpriteable
{
    public Sprite Sprite { get; internal set; }
    /// <summary>
    /// The object this property modifies.
    /// </summary>
    public ISpriteProperties Properties => Sprite;
    
    public SDL.SDL_Rect SourceRect()
        => new() {
            x = 0,
            y = 0,
            w = Sprite.Dimensions.x,
            h = Sprite.Dimensions.y
        };
}