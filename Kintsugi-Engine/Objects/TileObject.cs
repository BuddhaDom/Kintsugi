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
    /// <summary>
    /// Easing properties of this object.
    /// </summary>
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
        Easing = new TileObjectEasing();
    }


    /// <summary>
    /// Resolve any triggers that have collided at a given position.
    /// </summary>
    /// <param name="pos">Position on the grid of the trigger.</param>
    private void ResolveTriggerCollisions(Vec2Int pos)
    {
        if (Transform.Grid != null && Collider != null)
        {
            List<Collider> selfTriggers = new();
            List<Collider> otherTriggers = new();

            otherTriggers.AddRange(CollisionSystem.GetCollisionsColliderWithPosition(Collider, Transform.Grid, Transform.Position, true));
            var otherObjects = Transform.Grid.GetObjectsAtPosition(pos);
            if (otherObjects != null)
            {
                foreach (var tileObject in otherObjects)
                {
                    if (tileObject != this && tileObject.Collider != null)
                    {
                        if (CollisionSystem.CollidesColliderWithCollider(tileObject.Collider, Collider, true))
                        {
                            selfTriggers.Add(tileObject.Collider);
                        }
                    }
                }
                selfTriggers.AddRange(CollisionSystem.GetCollisionsGridAtPositionWithTileobjectsAtPosition(Transform.Grid, Transform.Position, true));
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
    /// <summary>
    /// Add a collider of type <typeparamref name="T"/> this type to this object with these parameter.
    /// </summary>
    /// <param name="belongLayers">The collision layers to which this object belongs to.</param>
    /// <param name="collideLayers">The collision layers this object should collide with.</param>
    /// <param name="isTrigger"><c>true</c> if this collider should act as a trigger.</param>
    /// <typeparam name="T">A type of <see cref="TileObjectCollider"/></typeparam>
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
    public void SetPosition(Vec2Int position, bool ease = true)
    {
        if (Transform.Grid != null)
            RemoveFromGridTileObjects(Transform.Grid);
        Transform.Position = position;
        if (ease)
        {
            Easing.BeginTowards(Transform.WorldSpacePosition);
        }
        else
        {
            Easing.TargetPosition = Transform.WorldSpacePosition;
            Easing.End();
        }
        if (Transform.Grid != null)
            AddToGridTileObjects(Transform.Grid);
        ResolveTriggerCollisions(position);
    }

    /// <summary>
    /// Move this object towards a target vector.
    /// </summary>
    /// <param name="vector">The direction to move to.</param>
    public void Move(Vec2Int vector, bool ease = true)
        => SetPosition(Transform.Position + vector, ease);

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
        Easing.StartPosition = Transform.WorldSpacePosition;
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

    /// <summary>
    /// Set a single sprite graphic to this object.
    /// </summary>
    /// <param name="sprite">Image form which to take the sprite.</param>
    public void SetSpriteSingle(Sprite sprite)
    {
        Graphic = new SpriteSingle();
        ((SpriteSingle)Graphic).Sprite = sprite;
    }

    /// <summary>
    /// Set a single sprite graphic to this object.
    /// </summary>
    /// <param name="spriteSingle">Sprite graphic to copy from.</param>
    public void SetSpriteSingle(SpriteSingle spriteSingle)
        => SetSpriteSingle(spriteSingle.Sprite);

    /// <summary>
    /// Sets an animation graphic for this object.
    /// </summary>
    /// <param name="spriteSheet">Sprite sheet from which to take frames.</param>
    /// <param name="timeLength">Duration of the animation.</param>
    /// <param name="frames">Frame indexes in the sprite sheet that make up the animation.</param>
    /// <param name="repeats">Amount of repetitions. Set to 0 if it should loop indefinitely.</param>
    /// <param name="bounces">Determines if the animation plays front then back once it reaches its last frame.</param>
    /// <param name="autoStart">Start the animation once this method ends?</param>
    public void SetAnimation(SpriteSheet spriteSheet, double timeLength, IEnumerable<int> frames,
        int repeats = 0, bool bounces = false, bool autoStart = true)
    {
        var animation = new Animation(timeLength, spriteSheet, frames, repeats, bounces);
        Graphic = animation;
        if (autoStart) ((Animation)Graphic).Start();
    }
        
    /// <summary>
    /// Sets an animation graphic for this object.
    /// </summary>
    /// <param name="path">Location of the image containing a sprite sheet.</param>
    /// <param name="spriteHeight">Height of a sprite.</param>
    /// <param name="spriteWidth">Width of a sprite.</param>
    /// <param name="spritesPerRow">Maximum amount of indexes in one row within sprite sheet.</param>
    /// <param name="timeLength">Duration of the animation.</param>
    /// <param name="frames">Frame indexes in the sprite sheet that make up the animation.</param>
    /// <param name="tilePivot">Pivot of the sprite. relative to its tile.</param>
    /// <param name="imagePivot">Pivot of the sprites relative to their image.</param>
    /// <param name="padding">Separation between sprite indexes.</param>
    /// <param name="margin">Separation between first sprite index and its image borders.</param>
    /// <param name="repeats">Amount of repetitions. Set to 0 if it should loop indefinitely.</param>
    /// <param name="bounces">Determines if the animation plays front then back once it reaches its last frame.</param>
    /// <param name="autoStart">Start the animation once this method ends?</param>
    public void SetAnimation(string path, int spriteHeight, int spriteWidth, int spritesPerRow, double timeLength,
        IEnumerable<int> frames, Vector2 tilePivot = default, Vector2 imagePivot = default,
        Vector2 padding = default, Vector2 margin = default, int repeats = 0, bool bounces = false, 
        bool autoStart = true)
        => SetAnimation(
            new SpriteSheet(path, spriteHeight, spriteWidth, spritesPerRow, tilePivot, imagePivot, padding, margin),
            timeLength, frames, repeats, bounces, autoStart
        );

    /// <summary>
    /// Sets an animation graphic for this object.
    /// </summary>
    /// <param name="animation">Animation from which to copy properties</param>
    /// <param name="autoStart">Start the animation once this method ends?</param>
    public void SetAnimation(Animation animation, bool autoStart = true)
        => SetAnimation(animation.SpriteSheet, animation.TimeLength, animation.FrameIndexes, animation.Repeats,
            animation.Bounces, autoStart);

    /// <summary>
    /// Sets the easing properties to this object.
    /// </summary>
    /// <param name="function">Easing function.</param>
    /// <param name="duration">Duration of the interpolation.</param>
    public void SetEasing(Easing.EasingFunction function,[Range(0,double.MaxValue)] double duration)
    {
        Easing.End();
        Easing.EasingFunction = function;
        Easing.StartPosition = Transform.WorldSpacePosition;
        Easing.TargetPosition = Transform.WorldSpacePosition;
        Easing.Duration = duration;
    }
        /*
        => Easing = new TileObjectEasing() 
        {
            EasingFunction = function,
            Duration = duration,
            StartPosition = Transform.WorldSpacePosition,
            TargetPosition = Transform.WorldSpacePosition
        };*/
}