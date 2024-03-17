namespace Kintsugi.Tiles;

/// <summary>
/// A representation of a tile within a grid layer.
/// </summary>
/// <param name="tileId">Global ID of the tile's graphic.</param>
/// <param name="tileSetId">ID of the tile set the graphic belongs to.</param>
public struct Tile
{
    public int TileId { get; }
    public bool IsEmpty => Id < 0;
    public static Tile Empty => new();
    /// <summary>
    /// Local ID of this tile's sprite in its tile set.
    /// </summary>
    public int Id { get; internal set; }
    /// <summary>
    /// ID of the tile set this tile uses.
    /// </summary>
    public int TileSetId { get; internal set; }

    public Tile(int tileId = -1, int tileSetId = -1)
    {
        TileId = tileId;
        TileSetId = tileSetId;
    }
}