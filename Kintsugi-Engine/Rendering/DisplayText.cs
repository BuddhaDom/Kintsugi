/*
*
*   The baseline functionality for getting text to work via SDL.   You could write your own text 
*       implementation (and we did that earlier in the course), but bear in mind DisplaySDL is built
*       upon this class.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using SDL2;
using System.Numerics;

namespace Kintsugi.Rendering
{

    // We'll be using SDL2 here to provide our underlying graphics system.
    public class TextDetails
    {
        string text;
        double x, y;
        SDL.SDL_Color col;
        int size;
        nint font;
        nint lblText;


        public TextDetails(string text, double x, double y, SDL.SDL_Color col, int spacing, Vector2 pivot)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.col = col;
            size = spacing;
            Pivot = pivot;
        }

        public string Text
        {
            get => text;
            set => text = value;
        }
        public double X
        {
            get => x;
            set => x = value;
        }
        public double Y
        {
            get => y;
            set => y = value;
        }
        public SDL.SDL_Color Col
        {
            get => col;
            set => col = value;
        }
        public int Size
        {
            get => size;
            set => size = value;
        }
        public nint Font { get => font; set => font = value; }
        public nint LblText { get => lblText; set => lblText = value; }
        public Vector2 Pivot { get; set; }
    }

    public class DisplayText : DisplayBase
    {
        protected nint _window, _rend;
        uint _format;
        int _access;
        private List<TextDetails> myTexts;
        private Dictionary<string, nint> fontLibrary;
        public override void ClearDisplay()
        {
            foreach (TextDetails td in myTexts)
            {
                SDL.SDL_DestroyTexture(td.LblText);
            }

            myTexts.Clear();
            SDL.SDL_SetRenderDrawColor(_rend, 0, 0, 0, 255);
            SDL.SDL_RenderClear(_rend);

        }

        public nint LoadFont(string path, int size)
        {
            string key = path + "," + size;

            if (fontLibrary.TryGetValue(key, out nint value))
            {
                return value;
            }

            fontLibrary[key] = SDL_ttf.TTF_OpenFont(path, size);
            return fontLibrary[key];
        }

        private void Update()
        {


        }

        private void Draw()
        {

            foreach (TextDetails td in myTexts)
            {

                SDL.SDL_Rect sRect;

                sRect.x = (int)td.X;
                sRect.y = (int)td.Y;
                sRect.w = 0;
                sRect.h = 0;


                SDL_ttf.TTF_SizeText(td.Font, td.Text, out sRect.w, out sRect.h);
                sRect.x -= (int)(sRect.w * td.Pivot.X);
                sRect.y -= (int)(sRect.h * td.Pivot.Y);

                SDL.SDL_RenderCopy(_rend, td.LblText, nint.Zero, ref sRect);

            }

            SDL.SDL_RenderPresent(_rend);

        }

        public override void Display()
        {

            Update();
            Draw();
        }

        public override void SetFullscreen()
        {
            SDL.SDL_SetWindowFullscreen(_window,
                 (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        }

        public override void Initialize()
        {
            fontLibrary = new Dictionary<string, nint>();

            SetSize(1280, 864);

            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            SDL_ttf.TTF_Init();
            _window = SDL.SDL_CreateWindow("Shard Game Engine",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                GetWidth(),
                GetHeight(),
                0);


            _rend = SDL.SDL_CreateRenderer(_window,
                -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);


            SDL.SDL_SetRenderDrawBlendMode(_rend, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            SDL.SDL_SetRenderDrawColor(_rend, 0, 0, 0, 255);


            myTexts = new List<TextDetails>();
        }



        public override void ShowText(string text, double x, double y, int size, int r, int g, int b, string fontPath = "Fonts/calibri.ttf", Vector2 pivot = default)
        {
            int nx, ny, w = 0, h = 0;

            nint font = LoadFont(fontPath, size);
            SDL.SDL_Color col = new()
            {
                r = (byte)r,
                g = (byte)g,
                b = (byte)b,
                a = 255
            };

            if (font == nint.Zero)
            {
                Debug.Log("TTF_OpenFont: " + SDL.SDL_GetError());
            }

            TextDetails td = new(text, x, y, col, 12, pivot)
            {
                Font = font
            };

            nint surf = SDL_ttf.TTF_RenderText_Blended(td.Font, td.Text, td.Col);
            nint lblText = SDL.SDL_CreateTextureFromSurface(_rend, surf);
            SDL.SDL_FreeSurface(surf);

            SDL.SDL_Rect sRect;

            sRect.x = (int)x;
            sRect.y = (int)y;
            sRect.w = w;
            sRect.h = h;

            SDL.SDL_QueryTexture(lblText, out _format, out _access, out sRect.w, out sRect.h);

            td.LblText = lblText;

            myTexts.Add(td);


        }
        public override void ShowText(char[,] text, double x, double y, int size, int r, int g, int b, string fontPath = "Fonts/calibri.ttf", Vector2 pivot = default)
        {
            string str = "";
            int row = 0;

            for (int i = 0; i < text.GetLength(0); i++)
            {
                str = "";
                for (int j = 0; j < text.GetLength(1); j++)
                {
                    str += text[j, i];
                }


                ShowText(str, x, y + row * size, size, r, g, b, fontPath, pivot);
                row += 1;

            }

        }
    }
}
