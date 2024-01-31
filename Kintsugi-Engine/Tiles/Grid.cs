using System.ComponentModel;
using Kintsugi.Core;
using Debug = System.Diagnostics.Debug;

namespace Kintsugi_Engine.Tiles;

public class Grid
{
    /// <summary>
    /// The tiles existing in this grid.
    /// </summary>
    public Tile[,] Tiles { get; }
    /// <summary>
    /// Length along the X axis.
    /// </summary>
    public int Width => Tiles.GetLength(0);
    /// <summary>
    /// Length along the Y axis.
    /// </summary>
    public int Height => Tiles.GetLength(1);

    /// <summary>
    /// Standard constructor to populate the grid with empty tiles.
    /// </summary>
    /// <param name="width">Width of the grid (X axis)</param>
    /// <param name="height">Height of the grid (Y axis)</param>
    public Grid(int width, int height)
    {
        this.Tiles = new Tile[height, width];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                Tiles[i, j] = new Tile(i, j, this);
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
        return (endTile.XPosition - startTile.XPosition, endTile.YPosition - startTile.YPosition);
    }
    
}
