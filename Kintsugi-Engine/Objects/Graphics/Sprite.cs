using System.Numerics;
using SixLabors.ImageSharp;

namespace Kintsugi.Objects.Graphics;

public struct Sprite : ISpriteProperties
{
    public Sprite(string path, int spriteHeight, int spriteWidth)
    {
        Path = path;
        SpriteHeight = spriteHeight;
        SpriteWidth = spriteWidth;
        
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
    public string Path { get; set; } = "";
    public int SpriteWidth { get; set; }
    public int SpriteHeight { get; set; }
    public Vector2 TilePivot { get; set; } = Vector2.Zero;
    public Vector2 ImagePivot { get; set; } = Vector2.Zero;
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }
}