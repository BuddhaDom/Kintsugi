using System.ComponentModel;
using Kintsugi.Core;
using TiledCS;

namespace Kintsugi.Tiles;

public struct Tile(Vec2Int position, GridLayer parent, int tileId = 0, int tileSetId = 0)
{
    /// <summary>
    /// Position of this tile on its layer.
    /// </summary>
    public Vec2Int Position { get; } = position;
    /// <summary>
    /// Layer to which this tile belongs to.
    /// </summary>
    public GridLayer ParentLayer { get; internal set; } = parent;
    /// <summary>
    /// Grid to which this tile belongs to.
    /// </summary>
    public Grid Grid => ParentLayer.ParentGrid;
    /// <summary>
    /// Width (in pixels) of this tile.
    /// </summary>
    public int TileWidth => ParentLayer.TileWidth;
    /// <summary>
    /// Local ID of this tile in its tile set.
    /// </summary>
    public int Id { get; internal set; } = tileId;
    /// <summary>
    /// ID of the tile set this tile uses.
    /// </summary>
    public int TileSetId { get; internal set; } = tileSetId;
}