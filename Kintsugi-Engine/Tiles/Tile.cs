using System.ComponentModel;
using Kintsugi.Core;
using TiledCS;

namespace Kintsugi.Tiles;

public struct Tile(Vec2Int position, int tileId = 0, int tileSetId = 0, GridLayer? parent = null)
{
    public Vec2Int Position { get; } = position;
    public GridLayer? ParentLayer { get; internal set; } = parent;
    public Grid? ParentGrid => ParentLayer?.ParentGrid;
    public int Id { get; internal set; } = tileId;
    public int TileSetId { get; internal set; } = tileSetId;
}