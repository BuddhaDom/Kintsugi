using System.Numerics;
using Kintsugi.Core;
using SixLabors.ImageSharp;

namespace Kintsugi.Objects.Graphics;

public struct SpriteSheet : ISpriteProperties
{
    public int SpritesPerRow { get; init; }
    public Vector2 Padding { get; }
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