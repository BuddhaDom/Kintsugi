using Kintsugi.Core;
using Kintsugi.EventSystem.Await;
using SDL2;
using System.Drawing;

namespace Kintsugi.Objects.Graphics;

/// <summary>
/// An animation graphic for <see cref="TileObject"/>.
/// </summary>
public class Animation : ISpriteable, IAwaitable
{
    /// <summary>
    /// Duration of the animation.
    /// </summary>
    public double TimeLength { get; set; }
    /// <summary>
    /// Number of times the animation should repeat. Set to <c>0</c> if it repeats indefinitely.
    /// </summary>
    public int Repeats { get; set; }
    /// <summary>
    /// Determines if the animation is meant to play backwards once it reaches its last frame.
    /// </summary>
    public bool Bounces { get; set; }
    /// <summary>
    /// Determines if the animation is currently playing.
    /// </summary>
    public bool Playing { get; private set; }
    /// <summary>
    /// Time at which the animation starts.
    /// </summary>
    public double StartTime { get; private set; }
    /// <summary>
    /// How long the animation has played for.
    /// </summary>
    public double PlayingTime => Playing ? Bootstrap.TimeElapsed - StartTime : 0d;
    /// <summary>
    /// Sprite sheet containing the frames relevant to the animation.
    /// </summary>
    public SpriteSheet SpriteSheet { get; internal set; }
    private IReadOnlyList<int> BounceFrameIndexes { get; set; } = [];
    private IReadOnlyList<int> frameIndexes = [];
    /// <summary>
    /// The list of frames that compose the animation.
    /// </summary>
    public IReadOnlyList<int> FrameIndexes
    {
        get => frameIndexes;
        set
        {
            frameIndexes = value;
            BounceFrameIndexes = new List<int>(value).Concat(value.AsEnumerable().Reverse().Skip(1).SkipLast(1)).ToList();
        }
    }
    
    public Animation(double timeLength, SpriteSheet graphic, IEnumerable<int> frames,
        int repeats = 0, bool bounces = false)
    {
        TimeLength = timeLength;
        SpriteSheet = graphic;
        Repeats = repeats;
        Bounces = bounces;
        FrameIndexes = frames.ToList();
    }

    /// <summary>
    /// Begin the animation.
    /// </summary>
    public void Start()
    {
        StartTime = Bootstrap.TimeElapsed;
        Playing = true;
    }

    /// <summary>
    /// End the animation.
    /// </summary>
    public void Stop()
    {
        Playing = false;
    }
    
    // TODO: Pause, End, and functions of the likes.
    
    // ==========================================
    //         ISpriteable Implementations
    // ==========================================
    public ISpriteProperties Properties => SpriteSheet;

    public bool IsFinished() => Playing == false;

    public SDL.SDL_Rect SourceRect()
    {
        int indexAtTime;

        if (Repeats != 0 && PlayingTime / TimeLength >= Repeats)
        {
            indexAtTime = Bounces ? 0 : FrameIndexes.Count - 1;
        }
        else
        {
            var localTime = PlayingTime % TimeLength;
            indexAtTime = (int)(localTime * (Bounces ? BounceFrameIndexes : FrameIndexes).Count / TimeLength);
        }

        int rectIndex = (Bounces ? BounceFrameIndexes : FrameIndexes)[indexAtTime];

        var coordinates = new Vec2Int(
            rectIndex % SpriteSheet.SpritesPerRow, 
            rectIndex / SpriteSheet.SpritesPerRow
        );
        
        return new SDL.SDL_Rect
        {
            w = SpriteSheet.Dimensions.x,
            h = SpriteSheet.Dimensions.y,
            x = (int) ((SpriteSheet.Dimensions.x + SpriteSheet.Padding.X) * coordinates.x + SpriteSheet.Margin.X),
            y = (int) ((SpriteSheet.Dimensions.y + SpriteSheet.Padding.Y) * coordinates.y + SpriteSheet.Margin.Y),
        };
    }

    public bool Flipped { get; set; }
    public Color ColorModulation { get; set; } = Color.White;
}