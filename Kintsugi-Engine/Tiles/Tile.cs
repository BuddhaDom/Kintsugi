using Kintsugi.Core;

namespace Kintsugi.Tiles;

public struct Tile(Vec2Int position, Grid parent)
{
    public Vec2Int Position { get; } = position;
    public Grid Parent { get; } = parent;
}