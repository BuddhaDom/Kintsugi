using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects.Graphics;
using Kintsugi.UI;
using TacticsGameTest.Units;

namespace TacticsGameTest.UI;

internal class HUD : Canvas
{
    private static HUD? hud;

    /// <summary>
    /// Get the HUD instance singleton.
    /// </summary>
    /// <returns>The existing or new HUD instance.</returns>
    public static HUD Instance => hud ??= new HUD();

    private HUD() => Bootstrap.GetInput().AddListener(this);
    
    /// <summary>
    /// Display information and buttons relevant to an actor.
    /// </summary>
    /// <param name="actor">Actor holding data relevant to the HUD.</param>
    public void DisplayActor(PlayerActor actor)
    {
        new FramedIcon(
            new Vector2(10f, 10f),
            new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("GUI\\characterFrame.png")),
            actor.Graphic is Animation a
                ? new Animation(a.TimeLength,
                    new SpriteSheet(
                        a.SpriteSheet.Path,
                        a.SpriteSheet.Dimensions.x,
                        a.SpriteSheet.Dimensions.y,
                        a.SpriteSheet.SpritesPerRow
                    ),
                    new[] { 0, 1, 2, 3 })
                : new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("guy.png")),
            Vector2.One * 3, Vector2.One * 5
        ).AddToCanvas(this);

        for (int i = 0; i < actor.abilities.Count; i++)
            new AbilityFrame(actor.abilities[i], i, new Vector2(
                40, // Left margin
                175 // Top margin
                + 100 // Spacing
                * i)).AddToCanvas(this);

        new StatFrame(
            "TEST", 5,
            new Vector2(150, 10),
            new Vector2(3, 3)
        ).AddToCanvas(this);
    }

    public void Clear() => Objects.Clear();
}