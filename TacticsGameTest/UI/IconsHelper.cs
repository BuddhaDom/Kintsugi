using Kintsugi.Core;

namespace TacticsGameTest.UI;

public static class IconsHelper
{
    public static string Get(int iconNumber) =>
        Bootstrap.GetAssetManager().GetAssetPath($"RPGIcons\\ico ({iconNumber}).png");
}