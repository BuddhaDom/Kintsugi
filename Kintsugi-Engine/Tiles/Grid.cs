using System.Drawing;
using Kintsugi.Core;
using Debug = System.Diagnostics.Debug;

namespace Kintsugi.Tiles;


public class Grid(int gridWidth, int gridHeight,  float tileWidth = 10.0f, 
    bool gridVisible = false, Color gridColor = default) : GameObject
{
    /// <summary>
    /// A 2D array containing the tiles existing in this grid.
    /// </summary>
    public Tile[,] Tiles { get; } = new Tile[gridWidth, gridHeight];

    /// <summary>
    /// Number of tiles along the X axis.
    /// </summary>
    public int Width => gridWidth;
    /// <summary>
    /// Number of tiles along the Y axis.
    /// </summary>
    public int Height => gridWidth;
    
    public override void Initialize()
    {
        // Populate tiles object.
        for (int y = 0; y < gridHeight; y++)
        for (int x = 0; x < gridWidth; x++)
            Tiles[x, y] = new Tile(new Vec2Int(x,y), this);
    }

    public override void Update()
    {
        if (gridVisible)
        {
            for (int i = 0; i <= gridHeight; i++) // Horizontal lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)Transform2D.X,
                    (int)(Transform2D.Y + tileWidth * i),
                    (int)(Transform2D.X + tileWidth * gridWidth),
                    (int)(Transform2D.Y + tileWidth * i),
                    gridColor
                );
            for (int i = 0; i <= gridWidth; i++) // Vertical lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)(Transform2D.X + tileWidth * i),
                    (int)Transform2D.Y,
                    (int)(Transform2D.X + tileWidth * i),
                    (int)(Transform2D.Y + tileWidth * gridHeight),
                    gridColor
                );
        }    
    }
}
