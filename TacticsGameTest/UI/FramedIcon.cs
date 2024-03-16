using System.Numerics;
using Kintsugi.Objects.Graphics;
using Kintsugi.UI;

namespace TacticsGameTest.UI;

public class FramedIcon : CanvasObject, IHUDObject
{
    private CanvasObject Icon { get; } = new();

    public FramedIcon(Vector2 position, ISpriteable frame, ISpriteable icon, Vector2 frameScale, Vector2 iconScale)
    {
        UIHelper.SetGraphic(frame, this);
        UIHelper.SetGraphic(icon, Icon);
        
        Graphic!.Scale = frameScale;
        Icon.Graphic!.Scale = iconScale;
        
        Position = position;
        if (Graphic != null && Icon.Graphic != null)
            Icon.Position = 
                position + (Dimensions * Graphic.Scale - Icon.Dimensions * Icon.Graphic.Scale) / 2;
    }

    public void AddToCanvas(Canvas canvas)
    {
        canvas.Objects.Add(Icon);
        canvas.Objects.Add(this);
    }
}