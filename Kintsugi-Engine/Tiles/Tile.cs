using System.ComponentModel;
using Kintsugi.Core;
using TiledCS;

namespace Kintsugi.Tiles;

public struct Tile(Vec2Int position, GridLayer parent, int tileId = 0, int tileSetId = 0)
{
    public Vec2Int Position { get; } = position;
    public GridLayer ParentLayer { get; internal set; } = parent;
    public Grid Grid => ParentLayer.ParentGrid!;
    public int TileWidth => ParentLayer.TileWidth;
    public int Id { get; internal set; } = tileId;
    public int TileSetId { get; internal set; } = tileSetId;
}