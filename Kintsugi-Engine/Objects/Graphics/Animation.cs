using Kintsugi.Core;
using SDL2;

namespace Kintsugi.Objects.Graphics;

public class Animation(TileObject parent) : ISpriteable
{
    public double TimeLength { get; set; }
    /// <summary>
    /// Number of times the animation should repeat. Set to <c>0</c> if it repeats indefinitely.
    /// </summary>
    public int Repeats { get; set; }
    public bool Bounces { get; set; }
    public bool Playing { get; private set; }
    public double StartTime { get; private set; }
    public double PlayingTime => Playing ? Bootstrap.TimeElapsed - StartTime : 0d;
    public SpriteSheet SpriteSheet { get; internal set; }
    private IReadOnlyList<int> BounceFrameIndexes { get; set; } = [];
    private IReadOnlyList<int> frameIndexes = [];
    public IReadOnlyList<int> FrameIndexes
    {
        get => frameIndexes;
        set
        {
            frameIndexes = value;
            BounceFrameIndexes = new List<int>(value).Concat(value.AsEnumerable().Reverse().Skip(1).SkipLast(1)).ToList();
        }
    }
    
    public Animation(TileObject parent, double timeLength, SpriteSheet graphic, IEnumerable<int> frames,
        int repeats = 0, bool bounces = false) : this(parent)
    {
        TimeLength = timeLength;
        SpriteSheet = graphic;
        Parent = parent;
        Repeats = repeats;
        Bounces = bounces;
        FrameIndexes = frames.ToList();
    }

    public void Start()
    {
        StartTime = Bootstrap.TimeElapsed;
        Playing = true;
    }

    public void Stop()
    {
        Playing = false;
    }
    
    // TODO: Pause, End, and functions of the likes.
    
    // ==========================================
    //         ISpriteable Implementations
    // ==========================================
    public TileObject Parent { get; set; } = parent;
    public ISpriteProperties Properties => SpriteSheet;
    
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
}