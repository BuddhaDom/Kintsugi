using Kintsugi.Core;
using TiledCS;

namespace Kintsugi.Tiles;

public struct Tile(Vec2Int position, Grid parent, int tileId = 0, int tileSetId = 0)
{
    public Vec2Int Position { get; } = position;
    public Grid Parent { get; } = parent;
    public int Id { get; } = tileId;
    public int TileSetId { get; } = tileSetId;
}