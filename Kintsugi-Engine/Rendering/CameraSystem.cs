using System.Numerics;

namespace Kintsugi.Rendering
{
    /// <summary>
    /// Camera system used by the game.
    /// </summary>
    public class CameraSystem
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
        } = 640;
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
        public Vector2 ScreenToWorldSpace(Vector2 screenSpace)
        {
            // Scale to world scale
            screenSpace *= (Height / _display.GetHeight());
            // Move to camera Position
            screenSpace += Position - new Vector2(Width, Height) * 0.5f;
            return screenSpace;
        }

        /// <summary>
        /// Converts a size in screenspace to a size in worldspace.
        /// </summary>
        public float ScreenToWorldSpaceSize(float screenspaceSize)
        {
            return screenspaceSize * (Height / _display.GetHeight());
        }

        /// <summary>
        /// Converts a point in worldspace to a point in screenspace.
        /// </summary>
        public Vector2 WorldToScreenSpace(Vector2 worldspace)
        {
            // Offset so the bottom left corner is at 0,0
            worldspace -= Position - new Vector2(Width, Height) * 0.5f;
            // Scale to fit screenspace
            worldspace *= (_display.GetHeight() / Height);
            // Probably should round to int here maybe?
            return worldspace;
        }

        /// <summary>
        /// Converts a size in worldspace to a size in screenspace.
        /// </summary>
        public float WorldToScreenSpaceSize(float worldspaceSize)
        {
            return worldspaceSize * (_display.GetHeight() / Height);
        }
        /// <summary>
        /// Aspect ratio of the camera. Bound to the aspect ratio of the window.
        /// </summary>
        public float AspectRatio { get => (float)_display.GetWidth() / (float)_display.GetHeight(); }

        internal CameraSystem(DisplayBase display)
        {
            _display = display;
        }

        private DisplayBase _display;

    }
}
