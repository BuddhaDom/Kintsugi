using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects.Graphics;
using TacticsGameTest.Abilities;

namespace TacticsGameTest.UI;

internal class AbilityFrame(Ability ability, int index)
{
    public Ability AbilityData { get; set; } = ability;

    public AbilityFramedIcon Icon { get; set; } = new(
        ability, index,
        new Vector2(20f, 50f),
        new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("GUI\\abilityFrame.png")),
        new SpriteSingle(ability.Path),
        Vector2.One, Vector2.One
    );

    internal class AbilityFramedIcon(
        Ability ability,
        int index,
        Vector2 position,
        ISpriteable frame,
        ISpriteable icon,
        Vector2 frameScale,
        Vector2 iconScale)
        : FramedIcon(position, frame, icon, frameScale, iconScale)
    {
        public override void OnClick()
        {
            base.OnClick();
            ability.actor.SelectAbility(index);
        }
    }
}