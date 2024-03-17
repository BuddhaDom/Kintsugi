using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Kintsugi.Core;
using Kintsugi.UI;

namespace TacticsGameTest.UI;

public class StatFrame : IHUDObject
{
    public Vector2 Position { get; }
    
    private CanvasObject Name { get; }
    private CanvasObject Number { get; }
    
    public StatFrame(string name, int value, Vector2 position, Vector2 scale)
    {
        Position = position;
        
        Number = new CanvasObject
        {
            TextColor = Color.FromArgb(184, 111, 80),
            FontSize = 24,
            FontPath = "Fonts\\calibri.ttf",
            TextPosition = new Vector2(17,13),
            Text = value.ToString(),
            Position = position
        };
        
        Number.SetSpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("GUI\\statFrame.png"));
        Number.Graphic!.Scale = scale;
        
        Name = new CanvasObject
        {
            TextColor = Color.FromArgb(184, 111, 80),
            FontSize = 24,
            FontPath = "Fonts\\calibri.ttf",
            TextPosition = new Vector2(17,13),
            Text = name,
            Position = position + Vector2.UnitX * Number.Dimensions * scale,
        };
        
        Name.SetSpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("GUI\\statName.png"));
        Name.Graphic!.Scale = scale;
    }

    public void AddToCanvas(Canvas canvas)
    {
        canvas.Objects.Add(Name);
        canvas.Objects.Add(Number);
    }
}