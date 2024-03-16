using Kintsugi.UI;
using System.Drawing;
using System.Numerics;
using Kintsugi.Objects.Graphics;

namespace TacticsGameTest.UI;

public class AbilityTooltipFrame : CanvasObject, IHUDObject
{
    public CanvasObject Title { get; }
    public CanvasObject Description { get; }

    public AbilityTooltipFrame(string title, string tooltip, ISpriteable frame, Vector2 position)
    {
        Position = position;
        UIHelper.SetGraphic(frame,this);
        Graphic!.Scale = Vector2.One * 3f;
        
        Title = new CanvasObject
        {
            Text = title,
            FontSize = 24,
            TextColor = Color.FromArgb(255, 184, 111, 80),
            Position = position + new Vector2(10f, 10f),
            FontPath = "Fonts\\calibri.ttf"
        };
        
        Description = new CanvasObject
        {
            Text = tooltip,
            FontSize = 16,
            TextColor = Color.FromArgb(255, 184, 111, 80),
            Position = position + new Vector2(10f, 30f),
            FontPath = "Fonts\\calibri.ttf"
        };

        Visible = false;
        Title.Visible = false;
        Description.Visible = false;
    }

    public void AddToCanvas(Canvas canvas)
    {
        canvas.Objects.Add(this);
        canvas.Objects.Add(Title);
        canvas.Objects.Add(Description);
    }
}