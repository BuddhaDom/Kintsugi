using System.ComponentModel;
using System.Drawing;
using Kintsugi.Core;
using Debug = System.Diagnostics.Debug;

namespace Kintsugi_Engine.Tiles;

public class Grid : GameObject
{
    /// <summary>
    /// The tiles existing in this grid.
    /// </summary>
    public required Tile[,] Tiles { get; set; }

    /// <summary>
    /// Length along the X axis.
    /// </summary>
    public int Width => Tiles.GetLength(0);
    /// <summary>
    /// Length along the Y axis.
    /// </summary>
    public int Height => Tiles.GetLength(1);
    
    public Grid(int gridWidth, int gridHeight,  float tileWidth = 1.0f, 
        bool gridVisible = false, Color gridColor = default)
    {
        InitializeTiles(gridWidth, gridHeight);
        if (gridVisible)
            for (int x = 0; x <= gridWidth; x++)
                // Logic
                return;
    }
    
    private void InitializeTiles(int width, int height)
    {
        // Initialize object.
        Tiles = new Tile[height, width];
        // Populate it.
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            Tiles[x, y] = new Tile((x,y), this);
    }
    
    /// <summary>
    /// Measure the offset between two tiles. <br/>
    /// Sends an error message to debug console if checking for tiles belonging to another grid.
    /// </summary>
    /// <param name="startTile">First tile.</param>
    /// <param name="endTile">Second tile.</param>
    /// <returns>Tuple representing the vector offset.</returns>
    public (int xOffset, int yOffset) DistanceBetween(Tile startTile, Tile endTile)
    {
        if (startTile.Parent != this || endTile.Parent != this)
            Debug.Fail("One of the tiles selected is not part of this grid.");
        return (
            endTile.Position.X - startTile.Position.X, 
            endTile.Position.Y - startTile.Position.Y
            );
    }
}
