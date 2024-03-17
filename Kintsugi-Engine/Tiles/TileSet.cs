namespace Kintsugi.Tiles;

public struct TileSet
{
    public string Source { get; }
    public int Width { get; }
    public int Height { get; }

    public TileSet(string source, int width, int height)
    {
        Source = source;
        Width = width;
        Height = height;
    }
}