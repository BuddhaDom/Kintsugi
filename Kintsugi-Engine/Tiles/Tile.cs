using System.ComponentModel;
using Kintsugi.Core;
using TiledCS;

namespace Kintsugi.Tiles;

public struct Tile(Vec2Int position, int tileId = 0, int tileSetId = 0)
{
    /// <summary>
    /// Position of this tile on its layer.
    /// </summary>
    public Vec2Int Position { get; } = position;
    
    /// <summary>
    /// Local ID of this tile in its tile set.
    /// </summary>
    public int Id { get; internal set; } = tileId;
    /// <summary>
    /// ID of the tile set this tile uses.
    /// </summary>
    public int TileSetId { get; internal set; } = tileSetId;
}