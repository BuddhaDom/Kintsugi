using System.Numerics;
using SixLabors.ImageSharp;

namespace Kintsugi.Objects.Graphics;

public struct Sprite : ISpriteProperties
{
    public Sprite(string path)
    {
        Path = path;
        
        // Get the Height and Width
        if (path == "") return;
        var image = Image.Load(path);
        ImageHeight = image.Height;
        ImageWidth = image.Width;
        image.Dispose();
        
        SpriteHeight = ImageHeight;
        SpriteWidth = ImageWidth;
    }
    
    // ===================================================
    //         ISpriteProperties  Implementation
    // ===================================================
    public string Path { get; set; } = "";
    public int SpriteWidth { get; set; }
    public int SpriteHeight { get; set; }
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }
    public Vector2 TilePivot { get; set; }
    public Vector2 ImagePivot { get; set; }
}