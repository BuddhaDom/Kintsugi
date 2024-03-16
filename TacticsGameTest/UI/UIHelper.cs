using Kintsugi.Core;
using Kintsugi.Objects.Graphics;

namespace TacticsGameTest.UI;

public static class UIHelper
{
    public static string Get(int iconNumber) =>
        Bootstrap.GetAssetManager().GetAssetPath($"RPGIcons\\ico ({iconNumber}).png");
    
    public static void SetGraphic(ISpriteable input, GraphicsObject target)
    {
        switch (input)
        {
            case SpriteSingle sprite:
                target.SetSpriteSingle(sprite);
                break;
            case Animation animation:
                target.SetAnimation(animation);
                break;
        }
    }
}