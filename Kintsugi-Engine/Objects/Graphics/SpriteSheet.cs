using System.Numerics;
using Kintsugi.Core;
using SixLabors.ImageSharp;

namespace Kintsugi.Objects.Graphics;

/// <summary>
/// A collection of sprites contained within one same image. These sprites should be uniformly spaced. 
/// </summary>
public struct SpriteSheet : ISpriteProperties
{
    /// <summary>
    /// How many sprite units make up a row in this sprite sheet. In case of variable lengths per row, should be the
    /// units in the longest row.
    /// </summary>
    public int SpritesPerRow { get; init; }
    /// <summary>
    /// Separation between each sprite.
    /// </summary>
    public Vector2 Padding { get; }
    /// <summary>
    /// Separation between the first sprite unit and the image borders.
    /// </summary>
    public Vector2 Margin { get; }

    public SpriteSheet(string path, int spriteHeight, int spriteWidth, int spritesPerRow, 
        Vector2 tilePivot = default, Vector2 imagePivot= default, Vector2 padding= default, Vector2 margin= default)
    {
        Path = path;
        Dimensions = new Vec2Int(spriteHeight, spriteWidth);
        TilePivot = tilePivot;
        ImagePivot = imagePivot;
        
        SpritesPerRow = spritesPerRow;
        Padding = padding;
        Margin = margin;
        
        // Get the Height and Width
        if (path == "") return;
        var image = Image.Load(path);
        ImageHeight = image.Height;
        ImageWidth = image.Width;
        image.Dispose();
    }

    // ===================================================
    //         ISpriteProperties  Implementation
    // ===================================================
    public string Path { get; set; }
    public Vec2Int Dimensions { get; set; }
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }
    public Vector2 TilePivot { get; set; }
    public Vector2 ImagePivot { get; set; }
}