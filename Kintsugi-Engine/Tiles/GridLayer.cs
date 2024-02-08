using System.ComponentModel;

namespace Kintsugi.Tiles;

public struct GridLayer
{
    public Tile[,] Tiles { get; }
    public Grid? ParentGrid { get; }
    public string Name { get; }
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
    public int TileWidth { get; }
    
    public GridLayer(Tile[,] tiles, int tileWidth, Grid parent, string name = "")
    {
        Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        ParentGrid = parent ?? throw new ArgumentNullException(nameof(parent));
        TileWidth = tileWidth;
        foreach (var tile in tiles)
            Tiles[tile.Position.x, tile.Position.y].ParentLayer = this;
        Name = name;
    }

    public GridLayer(int width, int height, int tileWidth, Grid parent, string name = "")
    {
        ParentGrid = parent ?? throw new ArgumentNullException(nameof(parent));
        TileWidth = tileWidth;
        Tiles = new Tile[width, height];
        Name = name;
    }
}