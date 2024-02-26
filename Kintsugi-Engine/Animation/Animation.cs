namespace Kintsugi.Animation;

public class Animation
{
    private float TimeLength { get; set; }
    private float FrameLength { get; set; }
}

public enum AnimationMode
{
    /// <summary>
    /// When the playtime finishes, repeat it from the start.
    /// </summary>
    Loop,
    /// <summary>
    /// When the playtime finishes, stay at the last frame.
    /// </summary>
    Single,
    /// <summary>
    /// When the playtime finishes, play the animation in reverse.
    /// When it goes back to the start, play it again normally.
    /// </summary>
    Bounce,
}