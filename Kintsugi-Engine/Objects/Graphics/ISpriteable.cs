using SDL2;

namespace Kintsugi.Objects.Graphics;

/// <summary>
/// Interface for objects that can be utilized as a <see cref="TileObject"/>'s <see cref="TileObject.Graphic"/> property.
/// </summary>
public interface ISpriteable
{
    /// <summary>
    /// Properties of the sprite which the <see cref="ISpriteable"/> borrows from.
    /// </summary>
    public ISpriteProperties Properties { get; }
    /// <summary>
    /// Determines the source rect which will be displayed on <see cref="Kintsugi.Rendering.DisplaySDL"/>.
    /// </summary>
    /// <returns></returns>
    public SDL.SDL_Rect SourceRect();
}