using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Tiles;

namespace Kintsugi.Objects.Properties;

/// <summary>
/// Transform properties of a tile object.
/// </summary>
public class TileObjectTransform(TileObject parent)
{
    /// <summary>
    /// Position of the tile object in a grid system. 
    /// </summary>
    public Vec2Int Position { get; internal set; } = Vec2Int.Zero;
    /// <summary>
    /// Direction the tile object is facing towards.
    /// </summary>
    public Facing Facing { get; internal set; } = Facing.East;
    /// <summary>
    /// Grid to which the tile object belongs to, if any.
    /// </summary>
    public Grid? Grid { get; internal set; }
    /// <summary>
    /// Layer to which the tile object belongs to in its grid, if any.
    /// </summary>
    public int Layer { get; internal set; }
    /// <summary>
    /// The object this property modifies.
    /// </summary>
    public TileObject Parent { get; } = parent;

    public Vector2 WorldSpacePosition => Grid == null ? Vector2.Zero : new Vector2(
        Grid.Position.X + Position.x * Grid.TileWidth,
        Grid.Position.Y + Position.y * Grid.TileWidth
    );
}