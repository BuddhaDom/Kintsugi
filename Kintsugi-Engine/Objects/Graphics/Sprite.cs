using System.Numerics;
using Kintsugi.Core;
using SixLabors.ImageSharp;

namespace Kintsugi.Objects.Graphics;

/// <summary>
/// A single sprite contained within a single image.
/// </summary>
public class Sprite : ISpriteProperties
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

        Dimensions = new Vec2Int(ImageWidth, ImageHeight);
    }
    
    // ===================================================
    //         ISpriteProperties  Implementation
    // ===================================================
    public string Path { get; set; } = "";
    public Vec2Int Dimensions { get; set; }
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }
    public Vector2 TilePivot { get; set; }
    public Vector2 ImagePivot { get; set; }
}