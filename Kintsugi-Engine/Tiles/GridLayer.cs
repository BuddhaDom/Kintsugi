using System.ComponentModel;
using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.Objects.Properties;

namespace Kintsugi.Tiles;

/// <summary>
/// A layer belonging to a grid, containing tiles.
/// </summary>
public struct GridLayer
{
    public Collider Collider { get; internal set; }

    /// <summary>
    /// Array of tiles belonging to this layer.
    /// </summary>
    public Tile[,] Tiles { get; }
    /// <summary>
    /// Name of this layer.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Create layer from a given <see cref="Tile"/> array.
    /// </summary>
    /// <param name="tiles">Two dimensional <see cref="Tile"/> array.</param>
    /// <param name="name">Name of this layer.</param>
    /// <exception cref="ArgumentNullException">
    /// If the <see cref="Tile"/> array is null.
    /// </exception>
    public GridLayer(Tile[,] tiles, string name = "")
    {
        Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        for (int y = 0; y < tiles.GetLength(1); y++)
            for (int x = 0; x < tiles.GetLength(0); x++)
                tiles[x, y] = Tile.Empty;
        Name = name;
    }

    /// <summary>
    /// Create an empty layer with a width and a height.
    /// </summary>
    /// <param name="width">Width of the grid.</param>
    /// <param name="height">Height of the grid.</param>
    /// <param name="name">Name of this layer.</param>
    public GridLayer(int width, int height, string name = "") : this(new Tile[width, height], name) { }

    /// <summary>
    /// Create an empty layer fitted for a specific grid.
    /// </summary>
    /// <param name="parent">Grid to which this layer should comply to.</param>
    /// <param name="name">Name of this layer.</param>
    public GridLayer(Grid parent, string name = "") : this(parent.GridWidth, parent.GridHeight, name) { }
}