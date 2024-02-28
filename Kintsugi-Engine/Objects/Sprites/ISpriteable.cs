using SDL2;

namespace Kintsugi.Objects.Sprites;

public interface ISpriteable
{
    public SDL.SDL_Rect SourceRect();
}