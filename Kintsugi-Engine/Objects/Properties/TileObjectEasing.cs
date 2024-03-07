using System.Numerics;
using Kintsugi.Core;
using Kintsugi.EventSystem.Await;
using TweenSharp.Animation;

namespace Kintsugi.Objects.Properties;

public class TileObjectEasing: IAwaitable
{
    public Easing.EasingFunction EasingFunction { get; internal set; } = Easing.Constant(1d);
    public double Duration { get; internal set; } = 1d;
    public double StartTime { get; private set; } = Bootstrap.TimeElapsed;
    public double PlayingTime => Playing ? Bootstrap.TimeElapsed - StartTime : 1d;
    public double Progress => Math.Clamp(PlayingTime / Duration, 0d, 1d);
    public bool Playing { get; private set; }
    public Vector2 StartPosition { get; internal set; }
    public Vector2 TargetPosition { get; internal set; }
    public Vector2 CurrentPosition => 
        (TargetPosition - StartPosition) * (float)Evaluate() + StartPosition;

    public bool IsFinished() => ((Bootstrap.TimeElapsed - StartTime) / Duration) >= 1;

    public double Evaluate(double progress) => EasingFunction(progress);
    public double Evaluate() => EasingFunction(Progress);
    
    public void BeginTowards(Vector2 targetPosition)
    {
        StartPosition = CurrentPosition;
        Playing = true;
        StartTime = Bootstrap.TimeElapsed;
        TargetPosition = targetPosition;
    }

    public void End()
    {
        Playing = false;
        StartPosition = TargetPosition;
    }
}