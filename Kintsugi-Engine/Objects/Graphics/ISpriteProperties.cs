using System.Numerics;
using Kintsugi.Core;

namespace Kintsugi.Objects.Graphics;

/// <summary>
/// Properties of an image that includes a sprite usable by an <see cref="ISpriteable"/>.
/// </summary>
public interface ISpriteProperties
{
    /// <summary>
    /// Path to the image file.
    /// </summary>
    public string Path { get; internal set; }
    /// <summary>
    /// Height and width of a sprite.
    /// </summary>
    public Vec2Int Dimensions { get; internal set; }
    /// <summary>
    /// Width of the image.
    /// </summary>
    public int ImageWidth { get; internal set; }
    /// <summary>
    /// Height of the image.
    /// </summary>
    public int ImageHeight { get; internal set; }
    /// <summary>
    /// Position on the tile from which the object is rendered.
    /// Defined between <see cref="Vector2.Zero"/> and <see cref="Vector2.One"/> as the upper and lower bounds of the tile width.
    /// </summary>
    public Vector2 TilePivot { get; internal set; }
    /// <summary>
    /// Position on the sprite which will match positions with the <see cref="TilePivot"/>.
    /// Defined between <see cref="Vector2.Zero"/> and this sprite's <see cref="ImageWidth"/> and <see cref="ImageHeight"/>. 
    /// </summary>
    public Vector2 ImagePivot { get; internal set; }
}