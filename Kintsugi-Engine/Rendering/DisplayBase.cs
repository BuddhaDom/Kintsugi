/*
*
*   The abstract DisplayBase class setting out the consistent interface all DisplayBase implementations need.  
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using System.Drawing;

namespace Kintsugi.Rendering
{
    public abstract class DisplayBase
    {
        protected int _height, _width;

        public virtual void DrawLine(int x, int y, int x2, int y2, int r, int g, int b, int a)
        {
        }

        public virtual void DrawLine(int x, int y, int x2, int y2, Color col)
        {
            DrawLine(x, y, x2, y2, col.R, col.G, col.B, col.A);
        }


        public virtual void DrawCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
        }

        public virtual void DrawCircle(int x, int y, int rad, Color col)
        {
            DrawCircle(x, y, rad, col.R, col.G, col.B, col.A);
        }

        public virtual void DrawFilledCircle(int x, int y, int rad, Color col)
        {
            DrawFilledCircle(x, y, rad, col.R, col.G, col.B, col.A);
        }

        public virtual void DrawFilledCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
            while (rad > 0)
            {
                DrawCircle(x, y, rad, r, g, b, a);
                rad -= 1;
            }
        }

        public void ShowText(string text, double x, double y, int size, Color col)
        {
            ShowText(text, x, y, size, col.R, col.G, col.B);
        }



        public virtual void SetFullscreen()
        {
        }

        public virtual void AddToDraw(GameObject gob)
        {
        }

        public virtual void RemoveToDraw(GameObject gob)
        {
        }
        public int GetHeight()
        {
            return _height;
        }

        public int GetWidth()
        {
            return _width;
        }

        public virtual void SetSize(int w, int h)
        {
            _height = h;
            _width = w;
        }

        public abstract void Initialize();
        public abstract void ClearDisplay();
        public abstract void Display();

        public abstract void ShowText(string text, double x, double y, int size, int r, int g, int b);
        public abstract void ShowText(char[,] text, double x, double y, int size, int r, int g, int b);
    }
}
