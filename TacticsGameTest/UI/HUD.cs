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
    /// Get the HUD instance singleton.
    /// </summary>
    /// <returns>The existing or new HUD instance.</returns>
    public static HUD Instance => _hud ??= new HUD();

    public FramedIcon Portrait { get; set; }
    public List<AbilityFrame> Abilities { get; set; } = [];
    public List<Heart> Health { get; set; }
    public SelectableActor SelectedActor { get; set; }

    public void UpdateFrom(SelectableActor actor)
    {
        SelectedActor = actor;
        Portrait = new FramedIcon(
            new Vector2(10f, 10f),
            new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("characterFrame.png")),
            actor.Graphic ?? new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("guy.png")),
            Vector2.One, Vector2.One
        );
        
        Portrait.AddToCanvas(this);

        for (int i = 0; i < Abilities.Count; i++)
        {
            
        }
        
        //TODO: Populate the GUI.
    }

    public void Clear() => Objects.Clear();
}