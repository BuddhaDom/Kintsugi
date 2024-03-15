using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects.Graphics;
using Kintsugi.UI;
using TacticsGameTest.Units;

namespace TacticsGameTest.UI;

internal class HUD : Canvas
{
    private static HUD? _hud = null;

    /// <summary>
    /// Get the HUD instance.
    /// </summary>
    /// <returns></returns>
    public static HUD GetHUD()
        => _hud ??= new HUD();
    
    public FramedIcon CharacterIcon { get; set; }
    public List<AbilityFrame> Abilities { get; set; }
    public List<Heart> Health { get; set; }

    public void UpdateFrom(SelectableActor actor)
    {
        CharacterIcon = new FramedIcon(
            new Vector2(10f, 10f),
            new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("characterFrame.png")),
            actor.Graphic ?? new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("guy.png")),
            Vector2.One, Vector2.One
        );
        
        Objects.Add(CharacterIcon);
        Objects.Add(CharacterIcon.Icon);
        
        //TODO: Populate the GUI.
    }

    public void ClearHUD()
        => Objects.Clear();
}