namespace Kintsugi_Engine.Tiles;

public class Grid(int height, int width)
{
    public Tile[,] Tiles { get; } = new Tile[height, width];
    public int Height => Tiles.GetLength(0);
    public int Width => Tiles.GetLength(1);
}