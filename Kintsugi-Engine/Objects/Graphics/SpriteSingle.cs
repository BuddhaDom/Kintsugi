using SDL2;

namespace Kintsugi.Objects.Graphics;

public class SpriteSingle(TileObject parent) : ISpriteable
{
    public Sprite Sprite { get; internal set; }
    /// <summary>
    /// The object this property modifies.
    /// </summary>
    public TileObject Parent { get; set; } = parent;
    public ISpriteProperties Properties => Sprite;
    
    public SDL.SDL_Rect SourceRect()
        => new() {
            x = 0,
            y = 0,
            w = Sprite.SpriteWidth,
            h = Sprite.SpriteHeight,
        };
}