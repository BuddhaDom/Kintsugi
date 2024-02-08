using System.Drawing;
using Kintsugi.Core;
using TiledCS;
namespace Kintsugi.Tiles;

/// <summary>
/// A <see cref="GameObject"/> containing a set of tiles to be rendered onto the scene through layers.
/// </summary>
public class Grid : GameObject
{
    /// <summary>
    /// A 2D array containing the tiles existing in this grid.
    /// </summary>
    public GridLayer[] Layers { get; }

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
    internal TileSet[] TileSets { get; }

    private readonly bool gridVisible;
    private readonly Color gridColor;

    public override void Initialize()
    {
        
    }

    /// <summary>
    /// Build a grid from a Tiled tilemap file.
    /// </summary>
    /// <param name="path">Path of a <c>.tmx</c> file containing tile map data.</param>
    /// <param name="gridVisible"><para>Default: <c>false</c></para><c>true</c> if the grid borders are to be displayed as well.</param>
    /// <param name="gridColor">Color of the grid borders if <paramref name="gridVisible"/> is <c>true</c>.</param>
    public Grid(string path, bool gridVisible = false, Color gridColor = default) 
    {
        // Construct values and properties.
        var tiledMap = new TiledMap(path);
        Width = tiledMap.Width;
        Height = tiledMap.Height;
        TileWidth = tiledMap.TileWidth;
        this.gridVisible = gridVisible;
        this.gridColor = gridColor;
        int c; // Generic counter.

        Layers = new GridLayer[tiledMap.Layers.Length];
        c = 0;
        foreach (var tiledLayer in tiledMap.Layers)
        {
            // Initialize this key in the Layer dictionary, as wel as Tile array.
            Layers[c] = new GridLayer(this, tiledLayer.name);
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            {
                var index = y * Width + x;
                var gid = tiledLayer.data[index];
                var tileSetIndex = GetTilesetIdFromGid(tiledMap, gid);
                
                // Set this tile in the layer dictionary.
                Layers[c].Tiles[x,y] = new Tile(new Vec2Int(x, y), 
                    gid - tiledMap.Tilesets[tileSetIndex].firstgid, tileSetIndex);
            }
            c++;
        }

        // Get the source paths for tilesets used by this grid.
        var dir = Path.GetDirectoryName(path);
        var tiledSets = tiledMap.GetTiledTilesets(dir+"/");
        TileSets = new TileSet[tiledSets.Count];
        c = 0;
        foreach (var image in tiledSets.Select(set => set.Value.Image))
        {
            TileSets[c] = new TileSet(
                Path.Combine(dir!,image.source),
                image.width, image.height);
            c++;
        }
    }

    // TODO: Generic constructor.
    // public Grid(int gridWidth, int gridHeight, int tileWidth, bool gridVisible = false, Color gridColor = default)
    // {
    //     this.gridWidth = gridWidth;
    //     this.gridHeight = gridHeight;
    //     this.TileWidth = tileWidth;
    //     Tiles = new Tile[gridWidth, gridHeight];
    // }

    public Grid(int width, int height, int tileWidth, string[] tileSetPaths, 
        GridLayer[]? layers = null, bool gridVisible = false, Color gridColor = default)
    {
        this.gridVisible = gridVisible;
        this.gridColor = gridColor;
        Width = width;
        Height = height;
        TileWidth = tileWidth;
        Layers = layers ?? Array.Empty<GridLayer>();
        TileSets = new TileSet[tileSetPaths.Length];
        for (int i = 0; i < tileSetPaths.Length; i++)
        {
            // TODO: Check each path to get width and height.
        }
    }

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