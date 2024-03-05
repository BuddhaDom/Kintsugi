using System.Numerics;
using Kintsugi.Core;

namespace Kintsugi.Objects.Graphics;

public interface ISpriteProperties
{
    public string Path { get; internal set; }
    public Vec2Int Dimensions { get; internal set; }
    public int ImageWidth { get; internal set; }
    public int ImageHeight { get; internal set; }
    public Vector2 TilePivot { get; internal set; }
    public Vector2 ImagePivot { get; internal set; }
}