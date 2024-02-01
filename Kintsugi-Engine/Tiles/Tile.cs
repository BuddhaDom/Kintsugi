namespace Kintsugi_Engine.Tiles;

public struct Tile((int X, int Y) position, Grid parent)
{
    public (int X, int Y) Position { get; } = position;
    public Grid Parent { get; } = parent;
}