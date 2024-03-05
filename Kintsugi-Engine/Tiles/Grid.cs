using System.Drawing;
using System.Numerics;
using Kintsugi.Collision;
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
    
    /// <summary>
    /// A dictionary containing coordinates and objects placed on that coordinate of this grid.
    /// </summary>
    internal Dictionary<Vec2Int, List<TileObject>> TileObjects { get; }

    /// <summary>
    /// Source location of the tile sets used by this grid.
    /// </summary>
    internal TileSet[] TileSets { get; }

    /// <summary>
    /// Should the grid borders be visible?
    /// </summary>
    private readonly bool gridVisible;
    /// <summary>
    /// What is the color of the grid border?
    /// </summary>
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

        Layers = new GridLayer[tiledMap.Layers.Length];
        var c = 0; // Generic counter.
        foreach (var tiledLayer in tiledMap.Layers)
        {
            // Initialize this key in the Layer dictionary, as wel as Tile array.
            Layers[c] = ParseTiledProperties(tiledLayer, new GridLayer(this, tiledLayer.name));
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

        GridLayer ParseTiledProperties(TiledLayer tiledLayer, GridLayer gridLayer)
        {

            var belong = new HashSet<string>();
            var collides = new HashSet<string>();
            var isTrigger = false;
            foreach (var prop in tiledLayer.properties)
            {
                var split = prop.name.Split(':');
                if (split.Length != 2) {
                    continue;
                }
                if (split[0].ToLower() == "kintsugi")
                {
                    var propertyName = split[1].ToLower();



                    switch (split[1].ToLower())
                    {

                        case "collisionlayerbelong":
                            belong.Add(prop.value);
                            break;
                        case "collisionlayercollide":
                            collides.Add(prop.value);

                            break;
                        case "istrigger":
                            isTrigger = true;
                            break;
                        default:
                            throw new Exception("Found Kintsugi property " + propertyName + " in " + tiledLayer + " but doesnt match any valid layer property");
                    }
                }
            }

            if (belong.Count != 0 || collides.Count != 0)
            {
                gridLayer.SetCollider(belong, collides, isTrigger);
            }

            return gridLayer;
        }
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
        TileObjects = new Dictionary<Vec2Int, List<TileObject>>();
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

    /// <summary>
    /// Calculates the cell in the grid that contains the given world position.
    /// </summary>
    public Vec2Int WorldToGridPosition(Vector2 worldPosition)
    {
        var localPosition = worldPosition - new Vector2(Transform.X, Transform.Y);

        var localPositionScaled = new Vector2(localPosition.X / TileWidth, localPosition.Y / TileWidth);
        Vec2Int gridPos = new Vec2Int((int)MathF.Floor(localPositionScaled.X), (int)MathF.Floor(localPositionScaled.Y));
        return gridPos;
    }
    /// <summary>
    /// Returns the world position of the upper left corner of the cell in the grid.
    /// </summary>
    public Vector2 GridToWorldPosition(Vec2Int gridPosition)
    {
        var scaledWorldPosition = gridPosition * TileWidth;
        return new Vector2(scaledWorldPosition.x, scaledWorldPosition.y) + new Vector2(Transform.X, Transform.Y);
    }
    /// <summary>
    /// Returns the world position of the center of the cell in the grid.
    /// </summary>
    public Vector2 GridCenterToWorldPosition(Vec2Int gridPosition)
    {
        var scaledWorldPosition = gridPosition * TileWidth;
        return new Vector2(scaledWorldPosition.x, scaledWorldPosition.y) + new Vector2(Transform.X, Transform.Y) + Vector2.One * TileWidth / 2f;
    }


    /// <summary>
    /// Get the Tileset ID from a given global ID.
    /// </summary>
    /// <param name="map">Tilemap this ob</param>
    /// <param name="gid"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// Precautionary checks of the tileset assets for developers.
    /// </summary>
    private void ValidateTileset()
    {
        foreach (var tileSet in TileSets)
        {
            if (tileSet.Width % TileWidth == 0 && tileSet.Height % TileWidth == 0) continue;
            Console.Error.WriteLine("Slicing warning. Provided tileset overflows when sliced by the provided Tile width.");
            Console.Error.WriteLine($"Remainder (px) - w:{tileSet.Width % TileWidth}, w:{tileSet.Height % TileWidth}");
        }
    }

    /// <summary>
    /// Get every <see cref="TileObject"/> at a specified coordinate, regardless of layer.
    /// </summary>
    /// <param name="position">Coordinate from which to query.</param>
    /// <returns>A collection of <see cref="TileObject"/> located in this position.</returns>
    public IReadOnlyList<TileObject>? GetObjectsAtPosition(Vec2Int position)
    {
        TileObjects.TryGetValue(position, out var result);
        return result;
    }

    /// <summary>
    /// Get the dictionary containing all <see cref="TileObject"/>s placed in this grid. 
    /// </summary>
    /// <returns>A dictionary of coordinates as keys, and collections of <see cref="TileObject"/> as values.</returns>
    public IReadOnlyDictionary<Vec2Int, List<TileObject>> GetObjects() => TileObjects;

    public bool IsGridPositionWithinGrid(Vec2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < GridWidth && gridPosition.y >= 0 && gridPosition.y < GridHeight;
    }
}