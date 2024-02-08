using System.Drawing;
using Kintsugi.Core;
using TiledCS;

namespace Kintsugi.Tiles;

public class Grid : GameObject
{
    /// <summary>
    /// A 2D array containing the tiles existing in this grid.
    /// </summary>
    public Dictionary<int, GridLayer> Layers { get; } = new();

    /// <summary>
    /// Number of tiles along the X axis.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Number of tiles along the Y axis.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Width (in pixels) of the tiles present in this grid.
    /// </summary>
    public int TileWidth { get; }

    /// <summary>
    /// Source location of the tile sets used by this grid.
    /// </summary>
    internal string[] TileSetSources { get; }

    private readonly bool gridVisible;
    private readonly Color gridColor;

    public override void Initialize()
    {
        
    }

    /// <summary>
    /// A <see cref="GameObject"/> containing a set of tiles to be rendered into a scene.
    /// </summary>
    /// <param name="path">Path of a <c>.tmx</c> file containing tile map data.</param>
    /// <param name="gridVisible"><para>Default: <c>false</c></para><c>true</c> if the grid borders are to be displayed as well.</param>
    /// <param name="gridColor">Color of the grid borders if <paramref name="gridVisible"/> is <c>true</c>.</param>
    public Grid(string path, bool gridVisible = false, Color gridColor = default) 
    {
        var tiledMap = new TiledMap(path);
        Width = tiledMap.Width;
        Height = tiledMap.Height;
        TileWidth = tiledMap.TileWidth;
        this.gridVisible = gridVisible;
        this.gridColor = gridColor;
        
        foreach (var tiledLayer in tiledMap.Layers)
        {
            Layers.Add(tiledLayer.id, new GridLayer(
                new Tile[Width, Height], TileWidth, this, tiledLayer.name));
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                var index = y * Width + x;
                var gid = tiledLayer.data[index];
                var tileSetIndex = GetTilesetIdFromGid(tiledMap, gid);
                
                Layers[tiledLayer.id].Tiles[x,y] = new Tile(new Vec2Int(x, y), Layers[tiledLayer.id],
                    gid - tiledMap.Tilesets[tileSetIndex].firstgid, tileSetIndex);
            }
        }
        
        TileSetSources = tiledMap.GetTiledTilesets(Path.GetDirectoryName(path)+"/")
            .Select(o => Path.Combine(
                Path.GetDirectoryName(path) ?? string.Empty,
                o.Value.Image.source)
            ).ToArray();
    }

    // public Grid(int gridWidth, int gridHeight, int tileWidth, bool gridVisible = false, Color gridColor = default)
    // {
    //     this.gridWidth = gridWidth;
    //     this.gridHeight = gridHeight;
    //     this.TileWidth = tileWidth;
    //     Tiles = new Tile[gridWidth, gridHeight];
    // }

    public override void Update()
    {
        if (gridVisible)
        {
            for (int i = 0; i <= Height; i++) // Horizontal lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)Transform2D.X,
                    (int)(Transform2D.Y + TileWidth * i),
                    (int)(Transform2D.X + TileWidth * Width),
                    (int)(Transform2D.Y + TileWidth * i),
                    gridColor
                );
            for (int i = 0; i <= Width; i++) // Vertical lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)(Transform2D.X + TileWidth * i),
                    (int)Transform2D.Y,
                    (int)(Transform2D.X + TileWidth * i),
                    (int)(Transform2D.Y + TileWidth * Height),
                    gridColor
                );
        }
        Bootstrap.GetDisplay().DrawGrid(this);
    }
    
    private static int GetTilesetIdFromGid(TiledMap map, int gid)
    {
        for (int i = 0; i < map.Tilesets.Length ; i++)
            if (i < map.Tilesets.Length-1)
            {
                int gid1 = map.Tilesets[i + 0].firstgid;
                int gid2 = map.Tilesets[i + 1].firstgid;
                if (gid >= gid1 && gid < gid2)
                    return i;
            }
            else
                return i;
        throw new Exception("Mismatch on Tilemap ID to GID information parsing.");
    }
}
