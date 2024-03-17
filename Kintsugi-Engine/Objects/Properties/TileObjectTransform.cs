using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Tiles;

namespace Kintsugi.Objects.Properties;

/// <summary>
/// Transform properties of a tile object.
/// </summary>
public class TileObjectTransform
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
    /// The object's current world space position.
    /// </summary>
    public Vector2 WorldSpacePosition 
        => Grid == null ? Vector2.Zero : Grid.Position + Position * Grid.TileWidth;
}