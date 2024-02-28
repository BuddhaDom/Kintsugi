using Kintsugi.Core;
using SDL2;

namespace Kintsugi.Objects.Sprites;

public class Animation : ISpriteable
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
    public float CurrentTime => Bootstrap.GetCurrentMillis() / 1000f - StartTime;
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
    
    public SDL.SDL_Rect SourceRect()
    {
        int indexAtTime;

        if (Repeats != 0 && (CurrentTime - StartTime) / TimeLength >= Repeats)
        {
            indexAtTime = ShouldBounce ? 0 : BounceFrameIndexes.Count;
        }
        else
        {
            var localTime = (CurrentTime - StartTime) % TimeLength;
            indexAtTime = (int)(localTime * (ShouldBounce ? BounceFrameIndexes : FrameIndexes).Count / TimeLength);
        }

        int rectIndex = (ShouldBounce ? BounceFrameIndexes : FrameIndexes)[indexAtTime];

        var coordinates = new Vec2Int(
            rectIndex % SpriteSheet.Width, 
            rectIndex / SpriteSheet.Width
        );
        
        return new SDL.SDL_Rect
        {
            h = SpriteSheet.SpriteHeight,
            w = SpriteSheet.SpriteWidth,
            x = (int) ((SpriteSheet.SpriteWidth + SpriteSheet.Padding.X) * coordinates.x + SpriteSheet.Margin.X),
            y = (int) ((SpriteSheet.SpriteHeight + SpriteSheet.Padding.Y) * coordinates.y + SpriteSheet.Margin.Y),
        };
    }
}