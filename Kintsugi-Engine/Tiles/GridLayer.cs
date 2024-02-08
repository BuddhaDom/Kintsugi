namespace Kintsugi.Tiles;

public struct GridLayer(Tile[,] tiles, Grid? parent = null, string name = "")
{
    public Tile[,] Tiles { get; } = tiles;
    public Grid? Parent { get; } = parent;
    public string Name { get; } = name;
}