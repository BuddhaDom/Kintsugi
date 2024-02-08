namespace Kintsugi.Tiles;

public struct GridLayer
{
    public Tile[,] Tiles { get; }
    public Grid? ParentGrid { get; }
    public string Name { get; }
    public int Width { get; }
    public int Height { get; }

    public GridLayer(Tile[,] tiles, Grid? parent = null, string name = "")
    {
        Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        foreach (var tile in tiles)
            Tiles[tile.Position.x, tile.Position.y].ParentLayer = this;
        ParentGrid = parent;
        Name = name;
        Height = tiles.GetLength(0);
        Width = tiles.GetLength(1);
    }

    public GridLayer(int width, int height, string name = "")
    {
        Tiles = new Tile[width, height];
        Width = width;
        Height = height;
        Name = name;
    }
}