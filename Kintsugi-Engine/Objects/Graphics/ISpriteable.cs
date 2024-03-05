using SDL2;

namespace Kintsugi.Objects.Graphics;

public interface ISpriteable
{
    public ISpriteProperties Properties { get; }
    public SDL.SDL_Rect SourceRect();
}