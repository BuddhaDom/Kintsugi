using Kintsugi.Core;

namespace Kintsugi_Engine.Tiles;

public struct Tile(Vec2Int position, Grid parent)
{
    public Vec2Int Position { get; } = position;
    public Grid Parent { get; } = parent;
}