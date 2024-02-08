using System.ComponentModel;
using Kintsugi.Core;

namespace Kintsugi.Tiles;

/// <summary>
/// A layer belonging to a grid, containing tiles.
/// </summary>
public struct GridLayer
{
    /// <summary>
    /// Array of tiles belonging to this layer.
    /// </summary>
    public Tile[,] Tiles { get; }
    /// <summary>
    /// Grid to which this layer belongs to.
    /// </summary>
    public Grid ParentGrid { get; }
    /// <summary>
    /// Name of this layer.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Number of tiles along the X axis.
    /// </summary>
    public int Width => ParentGrid.Width;
    /// <summary>
    /// Number of tiles along the Y axis.
    /// </summary>
    public int Height => ParentGrid.Height;

    /// <summary>
    /// Width (in pixels) of each tile in this layer.
    /// </summary>
    public int TileWidth => ParentGrid.TileWidth;
    
    /// <summary>
    /// Create from a given <see cref="Tile"/> array.
    /// </summary>
    /// <param name="tiles">Two dimensional <see cref="Tile"/> array.</param>
    /// <param name="parent">The <see cref="Grid"/> this layer belong to.</param>
    /// <param name="name">Name of this layer.</param>
    /// <exception cref="ArgumentNullException">
    /// If the <see cref="Tile"/> array or the parent <see cref="Grid"/> are null.
    /// </exception>
    public GridLayer(Tile[,] tiles, Grid parent, string name = "")
    {
        Tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        ParentGrid = parent ?? throw new ArgumentNullException(nameof(parent));
        Name = name;
    }

    /// <summary>
    /// Create an empty layer.
    /// </summary>
    /// <param name="parent">The <see cref="Grid"/> this layer belong to.</param>
    /// <param name="name">Name of this layer.</param>
    /// <exception cref="ArgumentNullException">
    /// If the <see cref="Tile"/> array or the parent <see cref="Grid"/> are null.
    /// </exception>
    public GridLayer(Grid parent, string name = "") : this(new Tile[parent.Width, parent.Height], parent, name)
    {
    }
}