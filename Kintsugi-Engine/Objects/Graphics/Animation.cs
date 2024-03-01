using Kintsugi.Core;
using SDL2;

namespace Kintsugi.Objects.Graphics;

public class Animation(TileObject parent) : ISpriteable
{
    public float TimeLength { get; set; }
    /// <summary>
    /// Number of times the animation should repeat. Set to <c>0</c> if it repeats indefinitely.
    /// </summary>
    public int Repeats { get; set; }
    public bool ShouldBounce { get; set; }
    public float StartTime { get; set; }
    public float CurrentTime => Bootstrap.GetCurrentMillis() / 1000f - StartTime;
    public SpriteSheet SpriteSheet { get; internal set; }
    private IReadOnlyList<int> frameIndexes = [];
    public IReadOnlyList<int> FrameIndexes
    {
        get => frameIndexes;
        set
        {
            frameIndexes = value;
            BounceFrameIndexes = new List<int>(value).Concat(value.AsEnumerable().Reverse().SkipLast(1)).ToList();
        }
    }
    private List<int> BounceFrameIndexes { get; set; } = [];
    
    public Animation(float timeLength, SpriteSheet graphic, IOrderedEnumerable<int> frames, TileObject parent,
        int repeats = 0, bool shouldBounce = false) : this(parent)
    {
        TimeLength = timeLength;
        SpriteSheet = graphic;
        Parent = parent;
        Repeats = repeats;
        ShouldBounce = shouldBounce;
        FrameIndexes = frames.ToList();
    }

    public Animation(float timeLength, SpriteSheet graphic, int fromIndex, int toIndex, TileObject parent, 
        int repeats = 0, bool shouldBounce = false) :
        this(timeLength, graphic, (IOrderedEnumerable<int>)Enumerable.Range(fromIndex, toIndex), 
            parent, repeats, shouldBounce) {}

    public void Start() => StartTime = Bootstrap.GetCurrentMillis() / 1000f;
    
    // TODO: Pause, End, and functions of the likes.
    
    // ==========================================
    //         ISpriteable Implementations
    // ==========================================
    public TileObject Parent { get; set; } = parent;
    public ISpriteProperties Properties => SpriteSheet;
    
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
            rectIndex % SpriteSheet.CellsPerRow, 
            rectIndex / SpriteSheet.CellsPerRow
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