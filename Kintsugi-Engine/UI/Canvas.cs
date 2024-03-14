using Kintsugi.Core;

namespace Kintsugi.UI;

public class Canvas : GameObject
{
    public List<CanvasObject> Objects { get; set; } = [];
    public int CurrentIndex { get; private set; }
    public CanvasObject CurrentObject => Objects[CurrentIndex];
    public bool Focused { get; set; }
    public bool Visible { get; set; } = true;

    public int MoveIndex(int units)
    {
        CurrentIndex = (CurrentIndex + units) % Objects.Count;
        return CurrentIndex;
    }

    public override void Update()
    {
        Bootstrap.GetDisplay().DrawCanvas(this);
    }
}