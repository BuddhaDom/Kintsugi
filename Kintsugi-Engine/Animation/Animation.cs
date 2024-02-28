using Kintsugi.Core;
using SDL2;

namespace Kintsugi.Animation;

public class Animation
{
    private float TimeLength { get; set; }
    public SpriteSheet SpriteSheet { get; set; }
    public List<int> FrameIndexes { get; }
    /// <summary>
    /// Number of times the animation should repeat. Set to <c>0</c> if it repeats indefinitely.
    /// </summary>
    public int Repeats { get; set; }
    public bool ShouldBounce { get; set; }

    public float StartTime { get; set; }
    public float CurrentTime { get; private set; }
    public float WorldStartTime { get; private set; }
    public float WorldCurrentTime => WorldStartTime + CurrentTime;
    private List<int> BounceFrameIndexes { get; }
    
    public Animation(float timeLength, SpriteSheet spriteSheet, IEnumerable<int> frames, int repeats = 0, bool shouldBounce = false)
    {
        TimeLength = timeLength;
        SpriteSheet = spriteSheet;
        Repeats = repeats;
        ShouldBounce = shouldBounce;
        FrameIndexes = frames.ToList();
        // :c
        BounceFrameIndexes = new List<int>(FrameIndexes).Concat(FrameIndexes.AsEnumerable().Reverse().SkipLast(1))
            .ToList();
    }

    public Animation(float timeLength, SpriteSheet spriteSheet, int fromIndex, int toIndex, int repeats = 0, bool shouldBounce = false) :
        this(timeLength, spriteSheet, Enumerable.Range(fromIndex, toIndex), repeats, shouldBounce) {}

    public SDL.SDL_Rect SourceRectAt(float time)
    {
        float localTime;
        int indexAtTime;

        if (Repeats != 0 && (time - WorldStartTime) / TimeLength >= Repeats)
        {
            localTime = TimeLength;
            indexAtTime = ShouldBounce ? 0 : BounceFrameIndexes.Count;
        }
        else
        {
            localTime = (time - StartTime) % TimeLength;
            indexAtTime = (int)(localTime * (ShouldBounce ? BounceFrameIndexes : FrameIndexes).Count / TimeLength);
        }

        var ss = SpriteSheet;

        int rectIndex = (ShouldBounce ? BounceFrameIndexes : FrameIndexes)[indexAtTime];

        var coordinates = new Vec2Int(
                rectIndex % ss.Width,
                rectIndex / ss.Width
            );
        
        return new SDL.SDL_Rect
        {
            h = ss.SpriteHeight,
            w = ss.SpriteWidth,
            x = (int) ((ss.SpriteWidth + ss.Padding.X) * coordinates.x + ss.Margin.X),
            y = (int) ((ss.SpriteHeight + ss.Padding.Y) * coordinates.y + ss.Margin.Y),
        };
    }
}