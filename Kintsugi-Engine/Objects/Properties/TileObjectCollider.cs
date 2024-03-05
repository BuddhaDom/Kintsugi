using Kintsugi.Collision;

namespace Kintsugi.Objects.Properties;

/// <summary>
/// <see cref="Collider"> on a <see cref="TileObject">. 
/// </summary>
public class TileObjectCollider : Collider, TileObjectColliderInitialize
{
    public TileObject TileObject { get; private set; }
    void TileObjectColliderInitialize.Initialize(TileObject t)
    {
        TileObject = t;
    }
}
