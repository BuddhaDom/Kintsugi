using System.Numerics;
using Kintsugi.Core;
using SixLabors.ImageSharp;

namespace Kintsugi.Animation;

public struct SpriteSheet
{
    public required string Path { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    public required int SpriteHeight { get; init; }
    public required int SpriteWidth { get; init; }
    public required int SheetPixelHeight { get; init; }
    public required int SheetPixelWidth { get; init; }
    public Vector2 Pivot { get; } = Vector2.Zero;
    public Vector2 Padding { get; }
    public Vector2 Margin { get; }

    public SpriteSheet(string path, int width, int height, int spriteHeight, int spriteWidth, Vector2 pivot, Vector2 padding, Vector2 margin)
    {
        Path = path;
        Width = width;
        Height = height;
        SpriteHeight = spriteHeight;
        SpriteWidth = spriteWidth;
        Padding = padding;
        Margin = margin;
        Pivot = pivot;
        
        // Get the Height and Width
        if (path == "") return;
        var image = Image.Load(path);
        SheetPixelHeight = image.Height;
        SheetPixelWidth = image.Width;
        image.Dispose();
    }

    public SpriteSheet(string path, int width, int height, int spriteHeight, int spriteWidth) :
        this(path, width, height, spriteHeight, spriteWidth, Vector2.Zero, Vector2.Zero, Vector2.Zero) {}
}