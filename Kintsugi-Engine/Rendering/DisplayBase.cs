/*
*
*   The abstract DisplayBase class setting out the consistent interface all DisplayBase implementations need.  
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using System.Drawing;
using Kintsugi.Tiles;

namespace Kintsugi.Rendering
{
    /// <summary>
    /// Abstract form of a display.
    /// </summary>
    public abstract class DisplayBase
    {
        protected int _height, _width;

        /// <summary>
        /// Draw a line to the display.
        /// </summary>
        /// <param name="x">Start x</param>
        /// <param name="y">Start y</param>
        /// <param name="x2">End x</param>
        /// <param name="y2">End y</param>
        /// <param name="r">Red color value</param>
        /// <param name="g">Green color value</param>
        /// <param name="b">Blue color value</param>
        /// <param name="a">Alpha color value</param>
        public virtual void DrawLine(int x, int y, int x2, int y2, int r, int g, int b, int a)
        {
        }

        /// <summary>
        /// Draw a line to the display.
        /// </summary>
        /// <param name="x">Start x</param>
        /// <param name="y">Start y</param>
        /// <param name="x2">End x</param>
        /// <param name="y2">End y</param>
        /// <param name="col">Color of the line.</param>
        public virtual void DrawLine(int x, int y, int x2, int y2, Color col)
        {
            DrawLine(x, y, x2, y2, col.R, col.G, col.B, col.A);
        }

        /// <summary>
        /// Draw a circle to the display.
        /// </summary>
        /// <param name="x">Center x</param>
        /// <param name="y">Center y</param>
        /// <param name="rad">Radius of circle</param>
        /// <param name="r">Red color value</param>
        /// <param name="g">Green color value</param>
        /// <param name="b">Blue color value</param>
        /// <param name="a">Alpha color value</param>
        public virtual void DrawCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
        }

        /// <summary>
        /// Draw a circle to the display.
        /// </summary>
        /// <param name="x">Center x</param>
        /// <param name="y">Center y</param>
        /// <param name="rad">Radius of circle</param>
        /// <param name="col">Color of the circle.</param>
        public virtual void DrawCircle(int x, int y, int rad, Color col)
        {
            DrawCircle(x, y, rad, col.R, col.G, col.B, col.A);
        }

        /// <summary>
        /// Draw a filled circle to the display.
        /// </summary>
        /// <param name="x">Center x</param>
        /// <param name="y">Center y</param>
        /// <param name="rad">Radius of circle</param>
        /// <param name="col">Color of the circle.</param>
        public virtual void DrawFilledCircle(int x, int y, int rad, Color col)
        {
            DrawFilledCircle(x, y, rad, col.R, col.G, col.B, col.A);
        }

        /// <summary>
        /// Draw a filled circle to the display.
        /// </summary>
        /// <param name="x">Center x</param>
        /// <param name="y">Center y</param>
        /// <param name="rad">Radius of circle</param>
        /// <param name="r">Red color value</param>
        /// <param name="g">Green color value</param>
        /// <param name="b">Blue color value</param>
        /// <param name="a">Alpha color value</param>
        public virtual void DrawFilledCircle(int x, int y, int rad, int r, int g, int b, int a)
        {
            while (rad > 0)
            {
                DrawCircle(x, y, rad, r, g, b, a);
                rad -= 1;
            }
        }

        /// <summary>
        /// Show text onto the display.
        /// </summary>
        /// <param name="text">Content of the message.</param>
        /// <param name="x">Position x</param>
        /// <param name="y">Position y</param>
        /// <param name="size">Font size</param>
        /// <param name="col">Color of text.</param>
        public void ShowText(string text, double x, double y, int size, Color col)
        {
            ShowText(text, x, y, size, col.R, col.G, col.B);
        }



        /// <summary>
        /// Fullscreens the game.
        /// </summary>
        public virtual void SetFullscreen()
        {
        }

        /// <summary>
        /// Adds a <see cref="GameObject"/> to the list of things to draw.
        /// </summary>
        /// <param name="gob">The object to display</param>
        public virtual void AddToDraw(GameObject gob)
        {
        }

        /// <summary>
        /// Remove a <see cref="GameObject"/> from the list of objects to display.
        /// </summary>
        /// <param name="gob">The object to display</param>
        public virtual void RemoveToDraw(GameObject gob)
        {
        }
        
        /// <summary>
        /// Get the height of the display.
        /// </summary>
        /// <returns>Height in pixels</returns>
        public int GetHeight()
        {
            return _height;
        }

        /// <summary>
        /// Get the width of the display.
        /// </summary>
        /// <returns>Width in pixels</returns>
        public int GetWidth()
        {
            return _width;
        }

        /// <summary>
        /// Set size of the display.
        /// </summary>
        /// <param name="w">Width in pixels</param>
        /// <param name="h">Height in pixels</param>
        public virtual void SetSize(int w, int h)
        {
            _height = h;
            _width = w;
        }

        /// <summary>
        /// Initialize the display.
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Clear the display of items.
        /// </summary>
        public abstract void ClearDisplay();
        /// <summary>
        /// Display items to draw.
        /// </summary>
        public abstract void Display();

        /// <summary>
        /// Show text onto the display.
        /// </summary>
        /// <param name="text">Content of the message.</param>
        /// <param name="x">Position x</param>
        /// <param name="y">Position y</param>
        /// <param name="size">Font size</param>
        /// <param name="r">Red color value</param>
        /// <param name="g">Green color value</param>
        /// <param name="b">Blue color value</param>
        public abstract void ShowText(string text, double x, double y, int size, int r, int g, int b);
        /// <summary>
        /// Show text onto the display.
        /// </summary>
        /// <param name="text">Content of the message.</param>
        /// <param name="x">Position x</param>
        /// <param name="y">Position y</param>
        /// <param name="size">Font size</param>
        /// <param name="r">Red color value</param>
        /// <param name="g">Green color value</param>
        public abstract void ShowText(char[,] text, double x, double y, int size, int r, int g, int b);
        
        /// <summary>
        /// Draw a grid into the display.
        /// </summary>
        /// <param name="grid">Grid to display.</param>
        public virtual void DrawGrid(Grid grid)
        {
        }
    }
}
