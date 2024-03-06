using Kintsugi.Collision;

namespace Kintsugi.Objects.Properties;

/// <summary>
/// <see cref="Collider"> on a <see cref="TileObject">. 
/// </summary>
public class TileObjectCollider : Collider, TileObjectColliderInitialize
{
    /// <summary>
    /// <see cref="TileObject"/> which this collider affects.
    /// </summary>
    public TileObject TileObject { get; private set; }
    void TileObjectColliderInitialize.Initialize(TileObject t)
    {
        TileObject = t;
    }
}
