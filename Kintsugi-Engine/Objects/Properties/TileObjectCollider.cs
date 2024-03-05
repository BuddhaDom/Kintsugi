namespace Kintsugi.Objects.Properties;

/// <summary>
/// Collision properties of a tile object.
/// </summary>
public class TileObjectCollider(TileObject parent)
{
    /// <summary>
    /// Collection of layers the object belongs to.
    /// </summary>
    public HashSet<string> BelongLayers { get; internal set; } = [];
    /// <summary>
    /// Collection of layers the tile object collides with.
    /// </summary>
    public HashSet<string> CollideLayers { get; internal set; } = [];
    /// <summary>
    /// <c>true</c> if the tile object is treated as a trigger collider.
    /// </summary>
    public bool IsTrigger { get; internal set; }
    /// <summary>
    /// The object this property modifies.
    /// </summary>
    public TileObject Parent { get; } = parent;
}