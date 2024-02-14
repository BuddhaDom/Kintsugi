using System.ComponentModel;
using Kintsugi.Core;
using TiledCS;

namespace Kintsugi.Tiles;

public struct Tile(int tileId = -1, int tileSetId = -1)
{
    public static Tile Empty => new();
    /// <summary>
    /// Local ID of this tile's sprite in its tile set.
    /// </summary>
    public int Id { get; internal set; } = tileId;
    /// <summary>
    /// ID of the tile set this tile uses.
    /// </summary>
    public int TileSetId { get; internal set; } = tileSetId;
}