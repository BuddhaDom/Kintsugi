using System.Numerics;
using SDL2;

namespace Kintsugi.Objects.Sprites;

public interface ISpriteable
{
    public SDL.SDL_Rect SourceRect();
    public nint Texture { get; }
    public Vector2 TilePivot { get; internal set; }
    public Vector2 ImagePivot { get; internal set; }
    public int SpriteWidth { get; internal set; }
    public int SpriteHeight { get; internal set; }
    public string Path { get; internal set; }
}