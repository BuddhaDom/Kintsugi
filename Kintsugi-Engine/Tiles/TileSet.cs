namespace Kintsugi.Tiles;

public struct TileSet(string source, int width, int height)
{
    public string Source { get; } = source;
    public int Width { get; } = width;
    public int Height { get; } = height;
}