using System.Drawing;
using Kintsugi.Core;
using Kintsugi.Rendering;
using TiledCS;

namespace Kintsugi.Tiles;

public class Grid : GameObject
{
    /// <summary>
    /// A 2D array containing the tiles existing in this grid.
    /// </summary>
    public Tile[,] Tiles { get; }

    /// <summary>
    /// Number of tiles along the X axis.
    /// </summary>
    public int Width => gridWidth;
    /// <summary>
    /// Number of tiles along the Y axis.
    /// </summary>
    public int Height => gridWidth;

    public int TileWidth { get; }

    internal string[] TileSetSources { get; }

    private int gridWidth, gridHeight;
    private bool gridVisible;
    private Color gridColor;
    private TiledMap tiledMap;
    
    public override void Initialize()
    {
        // Populate tiles object.
        for (int y = 0; y < gridHeight; y++)
        for (int x = 0; x < gridWidth; x++)
            // Tiles[x, y] = new Tile(new Vec2Int(x,y), this);
            return;
    }

    /// <summary>
    /// A <see cref="GameObject"/> containing a set of tiles to be rendered into a scene.
    /// </summary>
    /// <param name="path">Path of a <c>.tmx</c> file containing tile map data.</param>
    /// <param name="gridVisible"><para>Default: <c>false</c></para><c>true</c> if the grid borders are to be displayed as well.</param>
    /// <param name="gridColor">Color of the grid borders if <paramref name="gridVisible"/> is <c>true</c>.</param>
    public Grid(string path, bool gridVisible = false, Color gridColor = default) 
    {
        tiledMap = new TiledMap(path);
        gridWidth = tiledMap.Width;
        gridHeight = tiledMap.Height;
        TileWidth = tiledMap.TileWidth;
        this.gridVisible = gridVisible;
        this.gridColor = gridColor;

        Tiles = new Tile[gridWidth, gridHeight];
        
        foreach (var layer in tiledMap.Layers)
            for (int y = 0; y < gridHeight; y++)
            for (int x = 0; x < gridWidth; x++)
            {
                var index = y * gridWidth + x;
                var gid = layer.data[index];
                var tileSetIndex = GetTilesetIdFromGid(tiledMap, gid);
                Tiles[x, y] = new Tile(new Vec2Int(x, y), this, 
                    gid - tiledMap.Tilesets[tileSetIndex].firstgid, tileSetIndex);
                
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
            for (int i = 0; i <= gridHeight; i++) // Horizontal lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)Transform2D.X,
                    (int)(Transform2D.Y + TileWidth * i),
                    (int)(Transform2D.X + TileWidth * gridWidth),
                    (int)(Transform2D.Y + TileWidth * i),
                    gridColor
                );
            for (int i = 0; i <= gridWidth; i++) // Vertical lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)(Transform2D.X + TileWidth * i),
                    (int)Transform2D.Y,
                    (int)(Transform2D.X + TileWidth * i),
                    (int)(Transform2D.Y + TileWidth * gridHeight),
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
