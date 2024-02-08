/*
*
*   This is the implementation of the Simple Directmedia Layer through C#.   This isn't a course on 
*       graphics, so we're not going to roll our own implementation.   If you wanted to replace it with 
*       something using OpenGL, that'd be a pretty good extension to the base Shard engine.
*       
*   Note that it extends from DisplayText, which also uses SDL.  
*   
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using Kintsugi.Tiles;
using SDL2;

namespace Kintsugi.Rendering
{

    public class Line
    {
        private int sx, sy;
        private int ex, ey;
        private int r, g, b, a;

        public int Sx { get => sx; set => sx = value; }
        public int Sy { get => sy; set => sy = value; }
        public int Ex { get => ex; set => ex = value; }
        public int Ey { get => ey; set => ey = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a; set => a = value; }
    }

    public class Circle
    {
        int x, y, rad;
        private int r, g, b, a;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Radius { get => rad; set => rad = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a; set => a = value; }
    }


    public class DisplaySDL : DisplayText
    {
        private List<Transform> _toDraw;
        private List<Line> _linesToDraw;
        private List<Circle> _circlesToDraw;
        private List<Grid> _gridsToDraw;
        private Dictionary<string, nint> spriteBuffer;
        public override void Initialize()
        {
            spriteBuffer = new Dictionary<string, nint>();

            base.Initialize();

            _toDraw = new List<Transform>();
            _linesToDraw = new List<Line>();
            _circlesToDraw = new List<Circle>();
            _gridsToDraw = new List<Grid>();

        }

        public nint LoadTexture(Transform trans)
        {
            nint ret;
            uint format;
            int access;
            int w;
            int h;

            ret = LoadTexture(trans.SpritePath);

            SDL.SDL_QueryTexture(ret, out format, out access, out w, out h);
            trans.Ht = h;
            trans.Wid = w;
            trans.RecalculateCentre();

            return ret;

        }


        public nint LoadTexture(string path)
        {
            nint img;

            if (spriteBuffer.TryGetValue(path, out nint value))
            {
                return value;
            }

            img = SDL_image.IMG_Load(path);

            Debug.Log("IMG_Load: " + SDL_image.IMG_GetError());

            spriteBuffer[path] = SDL.SDL_CreateTextureFromSurface(_rend, img);

            SDL.SDL_SetTextureBlendMode(spriteBuffer[path], SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            return img;

        }


        public override void AddToDraw(GameObject gob)
        {
            _toDraw.Add(gob.Transform);

            if (gob.Transform.SpritePath == null)
            {
                return;
            }

            LoadTexture(gob.Transform.SpritePath);
        }

        public override void RemoveToDraw(GameObject gob)
        {
            _toDraw.Remove(gob.Transform);
        }


        void RenderCircle(int centreX, int centreY, int rad)
        {
            int dia = rad * 2;
            byte r, g, b, a;
            int x = rad - 1;
            int y = 0;
            int tx = 1;
            int ty = 1;
            int error = tx - dia;

            SDL.SDL_GetRenderDrawColor(_rend, out r, out g, out b, out a);

            // We Draw an octagon around the point, and then turn it a bit.  Do 
            // that until we have an outline circle.  If you want a filled one, 
            // do the same thing with an ever decreasing radius.
            while (x >= y)
            {

                SDL.SDL_RenderDrawPoint(_rend, centreX + x, centreY - y);
                SDL.SDL_RenderDrawPoint(_rend, centreX + x, centreY + y);
                SDL.SDL_RenderDrawPoint(_rend, centreX - x, centreY - y);
                SDL.SDL_RenderDrawPoint(_rend, centreX - x, centreY + y);
                SDL.SDL_RenderDrawPoint(_rend, centreX + y, centreY - x);
                SDL.SDL_RenderDrawPoint(_rend, centreX + y, centreY + x);
                SDL.SDL_RenderDrawPoint(_rend, centreX - y, centreY - x);
                SDL.SDL_RenderDrawPoint(_rend, centreX - y, centreY + x);

                if (error <= 0)
                {
                    y += 1;
                    error += ty;
                    ty += 2;
                }

                if (error > 0)
                {
                    x -= 1;
                    tx += 2;
                    error += tx - dia;
                }

            }
        }

        public override void DrawCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
            Circle c = new()
            {
                X = x,
                Y = y,
                Radius = rad,

                R = r,
                G = g,
                B = b,
                A = a
            };

            _circlesToDraw.Add(c);
        }
        public override void DrawLine(int x, int y, int x2, int y2, int r, int g, int b, int a)
        {
            Line l = new()
            {
                Sx = x,
                Sy = y,
                Ex = x2,
                Ey = y2,

                R = r,
                G = g,
                B = b,
                A = a
            };

            _linesToDraw.Add(l);
        }

        public override void Display()
        {

            SDL.SDL_Rect sRect;
            SDL.SDL_Rect tRect;



            foreach (Transform trans in _toDraw)
            {

                if (trans.SpritePath == null)
                {
                    continue;
                }

                var sprite = LoadTexture(trans);

                sRect.x = 0;
                sRect.y = 0;
                sRect.w = (int)(trans.Wid * trans.Scalex);
                sRect.h = (int)(trans.Ht * trans.Scaley);

                tRect.x = (int)trans.X;
                tRect.y = (int)trans.Y;
                tRect.w = sRect.w;
                tRect.h = sRect.h;

                SDL.SDL_RenderCopyEx(_rend, sprite, ref sRect, ref tRect, (int)trans.Rotz, nint.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
            }

            foreach (Circle c in _circlesToDraw)
            {
                SDL.SDL_SetRenderDrawColor(_rend, (byte)c.R, (byte)c.G, (byte)c.B, (byte)c.A);
                RenderCircle(c.X, c.Y, c.Radius);
            }

            foreach (Line l in _linesToDraw)
            {
                SDL.SDL_SetRenderDrawColor(_rend, (byte)l.R, (byte)l.G, (byte)l.B, (byte)l.A);
                SDL.SDL_RenderDrawLine(_rend, l.Sx, l.Sy, l.Ex, l.Ey);
            }

            foreach (var grid in _gridsToDraw)
            {
                foreach (var layer in grid.Layers)
                foreach (var tile in layer.Value.Tiles)
                {
                    if (tile.Id < 0) continue;
                    var tileSet = grid.TileSets[tile.TileSetId];
                    var source = tileSet.Source;
                    var sprite = LoadTexture(source);
                    var tileSetX = tile.Id % (tileSet.Width / grid.TileWidth);
                    var tileSetY = tile.Id / (tileSet.Width / grid.TileWidth);

                    sRect.x = tileSetX * grid.TileWidth;
                    sRect.y = tileSetY * grid.TileWidth;
                    sRect.w = grid.TileWidth;
                    sRect.h = grid.TileWidth;

                    tRect.x = (int)grid.Transform2D.X + tile.Position.x * grid.TileWidth;
                    tRect.y = (int)grid.Transform2D.Y + tile.Position.y * grid.TileWidth;
                    tRect.w = grid.TileWidth;
                    tRect.h = grid.TileWidth;
                    
                    SDL.SDL_RenderCopyEx(_rend, sprite, ref sRect, ref tRect, 0, nint.Zero, SDL.SDL_RendererFlip.SDL_FLIP_NONE);
                }
            }

            // Show it off.
            base.Display();


        }

        public override void ClearDisplay()
        {

            _toDraw.Clear();
            _circlesToDraw.Clear();
            _linesToDraw.Clear();
            _gridsToDraw.Clear();

            base.ClearDisplay();
        }

        public override void DrawGrid(Grid grid)
            => _gridsToDraw.Add(grid);
    }


}
