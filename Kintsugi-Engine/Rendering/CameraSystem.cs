using System.Numerics;

namespace Kintsugi.Rendering
{
    internal class CameraSystem
    {
        /// <summary>
        /// Center position of the camera in world space
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Size of the camera, measured as half its height in worldspace.
        /// </summary>
        public float Size
        {
            get; set;
        }
        /// <summary>
        /// Width of the camera in worldspace
        /// </summary>
        public float Width
        {
            get => AspectRatio * Height;
        }
        /// <summary>
        /// Height of the camera in worldspace
        /// </summary>
        public float Height
        {
            get => Size * 2f;
        }

        /// <summary>
        /// Converts a point in screenspace to a point in worldspace.
        /// </summary>
        public Vector2 ScreenToWorldSpace(int x, int y)
        {
            Vector2 worldSpace = new Vector2(x, y);
            // Scale to world scale
            worldSpace *= (Height / _display.GetHeight());
            // Move to camera Position
            worldSpace += Position - new Vector2(Width, Height) * 0.5f;
            return worldSpace;
        }
        /// <summary>
        /// Converts a point in worldspace to a point in screenspace.
        /// </summary>
        public Vector2 WorldToScreenSpace(Vector2 worldspace)
        {
            // Offset so the bottom left corner is at 0,0
            worldspace -= Position + new Vector2(Width, Height) * 0.5f;
            // Scale to fit screenspace
            worldspace *= (_display.GetHeight() / Height);
            // Probably should round to int here maybe?
            return worldspace;
        }
        /// <summary>
        /// Aspect ratio of the camera. Bound to the aspect ratio of the window.
        /// </summary>
        public float AspectRatio { get => _display.GetWidth() / _display.GetHeight(); }

        internal CameraSystem(DisplayBase display)
        {
            _display = display;
        }

        private DisplayBase _display;

    }
}
