using System.Numerics;
using Kintsugi.Core;
using Kintsugi.EventSystem.Await;
using TweenSharp.Animation;

namespace Kintsugi.Objects.Properties;

/// <summary>
/// Easing properties of a <see cref="TileObject"/>
/// </summary>
public class TileObjectEasing: IAwaitable
{
    /// <summary>
    /// Function that defines the easing movement of the object.
    /// </summary>
    public Easing.EasingFunction EasingFunction { get; internal set; } = Easing.Constant(1d);
    /// <summary>
    /// How long the movement easing takes to complete.
    /// </summary>
    public double Duration { get; internal set; } = 1d;
    /// <summary>
    /// Time at which the easing begins.
    /// </summary>
    public double StartTime { get; private set; } = Bootstrap.TimeElapsed;
    /// <summary>
    /// Time elapsed in the easing movement.
    /// </summary>
    public double PlayingTime => Playing ? Bootstrap.TimeElapsed - StartTime : Duration;
    /// <summary>
    /// Percentage of progress of the easing movement.
    /// </summary>
    public double Progress => Math.Clamp(PlayingTime / Duration, 0d, 1d);
    /// <summary>
    /// Determines whether the easing is playing.
    /// </summary>
    public bool Playing { get; private set; }
    /// <summary>
    /// Position at which the object is on movement start.
    /// </summary>
    public Vector2 StartPosition { get; internal set; }
    /// <summary>
    /// Position towards which the object is headed.
    /// </summary>
    public Vector2 TargetPosition { get; internal set; }
    /// <summary>
    /// Current position of the object given the easing function.
    /// </summary>
    public Vector2 CurrentPosition => 
        (TargetPosition - StartPosition) * (float)Evaluate() + StartPosition;

    /// <summary>
    /// Determines if the easing has finished.
    /// </summary>
    /// <returns></returns>
    public bool IsFinished() => ((Bootstrap.TimeElapsed - StartTime) / Duration) >= 1;

    /// <summary>
    /// Evaluate easing based on a given progress percentage.
    /// </summary>
    /// <param name="progress">Percentage of progress.</param>
    /// <returns>The corresponding value at this point in respect to the easing function.</returns>
    public double Evaluate(double progress) => EasingFunction(progress);
    /// <summary>
    /// Evaluate easing based on existing properties.
    /// </summary>
    /// <returns>The corresponding easing amount at this point in time in respect to the easing function.</returns>
    public double Evaluate() => EasingFunction(Progress);
    
    /// <summary>
    /// Start an easing function towards a target destination.
    /// </summary>
    /// <param name="targetPosition">Where the object is headed.</param>
    public void BeginTowards(Vector2 targetPosition)
    {
        StartPosition = CurrentPosition;
        Playing = true;
        StartTime = Bootstrap.TimeElapsed;
        TargetPosition = targetPosition;
    }

    /// <summary>
    /// Cut the easing function short abruptly.
    /// </summary>
    public void End()
    {
        Playing = false;
        StartPosition = TargetPosition;
    }
}