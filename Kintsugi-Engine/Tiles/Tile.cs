namespace Kintsugi_Engine.Tiles;

public struct Tile(int xPosition, int yPosition, Grid parent)
{
    public int XPosition { get; } = xPosition;
    public int YPosition { get; } = yPosition;
    public Grid Parent { get; } = parent;
}