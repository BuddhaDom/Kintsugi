using System.Drawing;
using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Rendering;
using SDL2;
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
    public int GridWidth { get; }

    /// <summary>
    /// Number of tiles along the Y axis.
    /// </summary>
    public int GridHeight { get; }

    /// <summary>
    /// Width (in pixels) of the tiles present in this grid.
    /// </summary>
    public int TileWidth { get; }
    
    public Dictionary<Vec2Int, List<TileObject>> TileObjects { get; }

    /// <summary>
    /// Source location of the tile sets used by this grid.
    /// </summary>
    internal TileSet[] TileSets { get; }

    private readonly bool gridVisible;
    private readonly Color gridColor;


    /// <summary>
    /// Build a grid from a Tiled tilemap file.
    /// </summary>
    /// <param name="tmxPath">Path of a <c>.tmx</c> file containing tile map data.</param>
    /// <param name="gridVisible"><para><c>false</c></para><c>true</c> if the grid borders are to be displayed as well.</param>
    /// <param name="gridColor">Color of the grid borders if <paramref name="gridVisible"/> is <c>true</c>.</param>
    /// <exception cref="ArgumentException">Thrown if the path is either not found or not a <c>.tmx</c> file.</exception>
    public Grid(string tmxPath, bool gridVisible = false, Color gridColor = default)
    {
        if (!Path.Exists(tmxPath)) 
            throw new ArgumentException("The provided file path does not exist.");
        if (Path.GetExtension(tmxPath) != ".tmx")
            throw new ArgumentException("The provided file path does not correspond to a .tmx file.");
        
        // Construct values and properties.
        var tiledMap = new TiledMap(tmxPath);
        GridWidth = tiledMap.Width;
        GridHeight = tiledMap.Height;
        TileWidth = tiledMap.TileWidth;
        TileObjects = new Dictionary<Vec2Int, List<TileObject>>();
        this.gridVisible = gridVisible;
        this.gridColor = gridColor;
        int c; // Generic counter.

        Layers = new GridLayer[tiledMap.Layers.Length];
        c = 0;
        foreach (var tiledLayer in tiledMap.Layers)
        {
            // Initialize this key in the Layer dictionary, as wel as Tile array.
            Layers[c] = new GridLayer(this, tiledLayer.name);
            for (int y = 0; y < GridHeight; y++)
            for (int x = 0; x < GridWidth; x++)
            {
                var index = y * GridWidth + x;
                var gid = tiledLayer.data[index];
                var tileSetIndex = GetTilesetIdFromGid(tiledMap, gid);
                
                // Set this tile in the layer dictionary.
                Layers[c].Tiles[x,y] = new Tile( 
                    gid - tiledMap.Tilesets[tileSetIndex].firstgid, 
                    tileSetIndex
                    );
            }
            c++;
        }

        // Get the source paths for tilesets used by this grid.
        var dir = Path.GetDirectoryName(tmxPath);
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
        ValidateTileset();
    }

    /// <summary>
    /// Build generic grid.
    /// </summary>
    /// <param name="gridWidth">Width of the grid.</param>
    /// <param name="gridHeight">Height of the grid.</param>
    /// <param name="tileWidth">Width of a tile in pixels.</param>
    /// <param name="tileSetPaths">Array of paths corresponding to the tileset textures.</param>
    /// <param name="layers">Set of layers existing in this grid.</param>
    /// <param name="gridVisible"><para><c>false</c></para><c>true</c> if the grid borders are to be displayed as well.</param>
    /// <param name="gridColor">Color of the grid borders if <paramref name="gridVisible"/> is <c>true</c>.</param>
    /// <exception cref="ArgumentException">Thrown if any of the paths are not found.</exception>
    public Grid(int gridWidth, int gridHeight, int tileWidth, string[] tileSetPaths, 
        GridLayer[]? layers = null, bool gridVisible = false, Color gridColor = default)
    {
        this.gridVisible = gridVisible;
        this.gridColor = gridColor;
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        TileWidth = tileWidth;
        Layers = layers ?? Array.Empty<GridLayer>();
        TileObjects = new Dictionary<Vec2Int, List<TileObject>?>();
        TileSets = new TileSet[tileSetPaths.Length];
        for (int i = 0; i < tileSetPaths.Length; i++)
        {
            var image = ((DisplaySDL)Bootstrap.GetDisplay()).LoadTexture(tileSetPaths[i]);
            SDL.SDL_QueryTexture(image, out _, out _, out int width, out int height);
            TileSets[i] = new TileSet(tileSetPaths[i], width, height);
        }
        ValidateTileset();
    }

    public override void Update()
    {
        if (gridVisible)
        {
            for (int i = 0; i <= GridHeight; i++) // Horizontal lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)Transform2D.X,
                    (int)(Transform2D.Y + TileWidth * i),
                    (int)(Transform2D.X + TileWidth * GridWidth),
                    (int)(Transform2D.Y + TileWidth * i),
                    gridColor
                );
            for (int i = 0; i <= GridWidth; i++) // Vertical lines
                Bootstrap.GetDisplay().DrawLine(
                    (int)(Transform2D.X + TileWidth * i),
                    (int)Transform2D.Y,
                    (int)(Transform2D.X + TileWidth * i),
                    (int)(Transform2D.Y + TileWidth * GridHeight),
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

    private void ValidateTileset()
    {
        foreach (var tileSet in TileSets)
        {
            if (tileSet.Width % TileWidth == 0 && tileSet.Height % TileWidth == 0) continue;
            Console.Error.WriteLine("Slicing warning. Provided tileset overflows when sliced by the provided Tile width.");
            Console.Error.WriteLine($"Remainder (px) - w:{tileSet.Width % TileWidth}, w:{tileSet.Height % TileWidth}");
        }
    }
}