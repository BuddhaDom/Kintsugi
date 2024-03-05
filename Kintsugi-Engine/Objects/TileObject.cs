using System.Numerics;
using Kintsugi.Collision;
using System.ComponentModel.DataAnnotations;
using Kintsugi.Core;
using Kintsugi.Objects.Graphics;
using Kintsugi.Objects.Properties;
using Kintsugi.Tiles;
using TweenSharp.Animation;

namespace Kintsugi.Objects;

/// <summary>
/// An object that can be used and placed into a <see cref="Grid"/>. 
/// </summary>
public class TileObject
{
    /// <summary>
    /// Transform properties of this object.
    /// </summary>
    public TileObjectTransform Transform { get; private set; }
    public TileObjectEasing Easing { get; private set; }
    /// <summary>
    /// Collision properties of this object.
    /// </summary>
    public TileObjectCollider? Collider { get; private set; }
    /// <summary>
    /// Graphic properties of this object.
    /// </summary>
    public ISpriteable? Graphic { get; private set; }
        
    /// <summary>
    /// Creates a <see cref="TileObject"/> with a default Transform property. 
    /// </summary>
    public TileObject()
    {
        Transform = new TileObjectTransform(this);
        Easing = new TileObjectEasing(this);
    }


    private void ResolveTriggerCollisions(Vec2Int pos)
    {
        if (Transform.Grid != null && Collider != null)
        {
            List<Collider> selfTriggers = new();
            List<Collider> otherTriggers = new();

            otherTriggers.AddRange(CollisionSystem.GetCollidingTriggersColliderWithPosition(Collider, Transform.Grid, Transform.Position));
            var otherObjects = Transform.Grid.GetObjectsAtPosition(pos);
            if (otherObjects != null)
            {
                foreach (var tileObject in otherObjects)
                {
                    if (tileObject != this && tileObject.Collider != null)
                    {
                        if (CollisionSystem.TriggerCollidesColliderWithCollider(tileObject.Collider, Collider))
                        {
                            selfTriggers.Add(tileObject.Collider);
                        }
                    }
                }
                selfTriggers.AddRange(CollisionSystem.GetCollidingTriggersGridAtPositionWithTileobjectsAtPosition(Transform.Grid, Transform.Position));
            }
            
            foreach (var selfTrigger in selfTriggers)
            {
                selfTrigger.OnTriggerCollision(Collider);
            }
            foreach (var otherTrigger in otherTriggers)
            {
                Collider.OnTriggerCollision(otherTrigger);
            }

        }
    }

    /// <summary>
    /// Add a collider property to this object with these parameters.
    /// </summary>
    /// <param name="belongLayers">The collision layers to which this object belongs to.</param>
    /// <param name="collideLayers">The collision layers this object should collide with.</param>
    /// <param name="isTrigger"><c>true</c> if this collider should act as a trigger.</param>
    public void SetCollider(HashSet<string> belongLayers, HashSet<string> collideLayers, bool isTrigger = false)
    {
        Collider ??= new TileObjectCollider();
        ((TileObjectColliderInitialize)Collider).Initialize(this);
        Collider.IsTrigger = isTrigger;
        Collider.BelongLayers = belongLayers;
        Collider.CollideLayers = collideLayers;
    }
    public void SetColliderTyped<T>(HashSet<string> belongLayers, HashSet<string> collideLayers, bool isTrigger = false)
        where T: TileObjectCollider, TileObjectColliderInitialize, new()
    {
        Collider = new T();
        ((TileObjectColliderInitialize)Collider).Initialize(this);
        Collider.IsTrigger = isTrigger;
        Collider.BelongLayers = [..belongLayers];
        Collider.CollideLayers = [..collideLayers];
    }

    /// <summary>
    /// Add a collider property to this property to be copied from another <see cref="TileObjectCollider"/>.
    /// </summary>
    /// <param name="collider">Collider object to copy from.</param>
    public void SetCollider(TileObjectCollider collider)
        => SetCollider([..collider.BelongLayers], [..collider.CollideLayers], collider.IsTrigger);
    /// <summary>
    /// Establish the position of this object in a grid system. This method also updates the grid's
    /// <see cref="Grid.TileObjects"/> dictionary.
    /// </summary>
    /// <param name="position">New coordinates of the object.</param>
    public void SetPosition(Vec2Int position)
    {
        if (Transform.Grid != null)
            RemoveFromGridTileObjects(Transform.Grid);
        Transform.Position = position;
        Easing.BeginTowards(Transform.WorldSpacePosition);
        if(Transform.Grid != null)
            AddToGridTileObjects(Transform.Grid);
        ResolveTriggerCollisions(position);
    }

    /// <summary>
    /// Move this object towards a target vector.
    /// </summary>
    /// <param name="vector">The direction to move to.</param>
    public void Move(Vec2Int vector)
        => SetPosition(Transform.Position + vector);

    /// <summary>
    /// Remove this objects grid. Does nothing if the grid is already <c>null</c>.
    /// </summary>
    public void RemoveFromGrid()
    {
        if (Transform.Grid == null) return;
        RemoveFromGridTileObjects(Transform.Grid);
        Transform.Grid = null;
    }

    /// <summary>
    /// Remove this object from the target grid's <see cref="Grid.TileObjects"/> property.
    /// </summary>
    /// <param name="grid">Target grid to affect.</param>
    private void RemoveFromGridTileObjects(Grid grid)
    {
        ArgumentNullException.ThrowIfNull(grid);
            
        if (grid.TileObjects.TryGetValue(Transform.Position, out _))
            grid.TileObjects[Transform.Position].Remove(this);
        if (grid.TileObjects[Transform.Position].Count == 0)
            grid.TileObjects.Remove(Transform.Position);
    }

    /// <summary>
    /// Add this tile object to a grid, and on a specific layer.
    /// </summary>
    /// <param name="grid">Target grid to place the object in.</param>
    /// <param name="layer">Layer of the grid to which this tile object will belong to.</param>
    public void AddToGrid(Grid grid, int layer)
    {
        ArgumentNullException.ThrowIfNull(grid);
        AddToGridTileObjects(grid);
        Transform.Grid = grid;
        Transform.Layer = layer;
    }

    /// <summary>
    /// Add this object to a target grid's <see cref="Grid.TileObjects"/> property.
    /// </summary>
    /// <param name="grid">Target grid to affect.</param>
    private void AddToGridTileObjects(Grid grid)
    {
        if (grid.TileObjects.TryGetValue(Transform.Position, out var value))
            value.Add(this);
        else
            grid.TileObjects.Add(Transform.Position, [this]);
    }

    /// <summary>
    /// Set the sprite properties for this object.
    /// </summary>
    /// <param name="path">Path to the sprite's graphic.</param>
    /// <param name="tilePivot">
    /// Position on the tile from which the object is rendered. Defined between <see cref="Vector2.Zero"/> and
    /// <see cref="Vector2.One"/> as the upper and lower bounds of the tile width. </param>
    /// <param name="imagePivot">
    /// Position on the sprite which will match positions with the <paramref name="tilePivot"/>.
    /// Defined between <see cref="Vector2.Zero"/> the pixel width and height of the sprite.</param>
    public void SetSpriteSingle(string path, Vector2 tilePivot = default, Vector2 imagePivot = default) 
    {
        SetSpriteSingle(new Sprite(path));
        Graphic!.Properties.TilePivot = tilePivot;
        Graphic.Properties.ImagePivot = imagePivot;
    }

    public void SetSpriteSingle(Sprite sprite)
    {
        Graphic ??= new SpriteSingle(this);
        ((SpriteSingle)Graphic).Sprite = sprite;
    }

    public void SetSpriteSingle(SpriteSingle spriteSingle)
        => SetSpriteSingle(spriteSingle.Sprite);

    public void SetAnimation(SpriteSheet spriteSheet, double timeLength, IEnumerable<int> frames,
        int repeats = 0, bool bounces = false, bool autoStart = true)
    {
        var animation = new Animation(this, timeLength, spriteSheet, frames, repeats, bounces);
        Graphic ??= animation;
        if (autoStart) ((Animation)Graphic).Start();
    }
        
    public void SetAnimation(string path, int spriteHeight, int spriteWidth, int spritesPerRow, double timeLength,
        IEnumerable<int> frames, Vector2 tilePivot = default, Vector2 imagePivot = default,
        Vector2 padding = default, Vector2 margin = default, int repeats = 0, bool bounces = false, 
        bool autoStart = true)
        => SetAnimation(
            new SpriteSheet(path, spriteHeight, spriteWidth, spritesPerRow, tilePivot, imagePivot, padding, margin),
            timeLength, frames, repeats, bounces, autoStart
        );

    public void SetAnimation(Animation animation, bool autoStart = true)
        => SetAnimation(animation.SpriteSheet, animation.TimeLength, animation.FrameIndexes, animation.Repeats,
            animation.Bounces, autoStart);

    public void SetEasing(Easing.EasingFunction function,[Range(0,double.MaxValue)] double duration)
        => Easing = new TileObjectEasing(this) 
        {
            EasingFunction = function,
            Duration = duration,
            StartPosition = Transform.WorldSpacePosition,
            TargetPosition = Transform.WorldSpacePosition
        };
}