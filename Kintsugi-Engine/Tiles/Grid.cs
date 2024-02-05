using System.Drawing;
using Kintsugi.Core;
using Debug = System.Diagnostics.Debug;

namespace Kintsugi.Tiles;

/// <summary>
/// A <see cref="GameObject"/> containing a set of tiles to be rendered into a scene.
/// </summary>
/// <param name="gridWidth">The width (in tiles) of the grid.</param>
/// <param name="gridHeight">The height (in tiles) of the grid.</param>
/// <param name="tileWidth"><para>Default: <c>10.0f</c></para>The uniform size of tiles.</param>
/// <param name="gridVisible"><para>Default: <c>false</c></para><c>true</c> if the grid borders are to be displayed as well.</param>
/// <param name="gridColor">Color of the grid borders if <paramref name="gridVisible"/> is <c>true</c>.</param>
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
