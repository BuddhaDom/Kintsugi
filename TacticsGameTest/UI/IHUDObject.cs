using Kintsugi.UI;

namespace TacticsGameTest.UI;

/// <summary>
/// Simple class for classes that should have their own way to add themselves to a canvas.
/// </summary>
public interface IHUDObject
{
    /// <summary>
    /// Determines how a class should add itself to a canvas.
    /// </summary>
    /// <param name="canvas">Canvas to add this class to.</param>
    public void AddToCanvas(Canvas canvas);
}