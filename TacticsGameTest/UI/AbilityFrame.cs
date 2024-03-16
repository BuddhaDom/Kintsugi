using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects.Graphics;
using Kintsugi.UI;
using TacticsGameTest.Abilities;

namespace TacticsGameTest.UI;

internal class AbilityFrame(Ability ability, int index, Vector2 position) : IHUDObject
{
    private FramedIcon Icon { get; } = new AbilityFramedIcon(
        ability, index, position,
        new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("GUI\\abilityFrame.png")),
        new SpriteSingle(ability.Path),
        Vector2.One * 5, Vector2.One * 2.25f
    );

    public void AddToCanvas(Canvas canvas)
    {
        Icon.AddToCanvas(canvas);
    }

    private class AbilityFramedIcon : FramedIcon
    {
        private Ability Ability { get; }
        private int Index { get; }
        
        public AbilityFramedIcon(Ability ability, int index, Vector2 position, ISpriteable frame, ISpriteable icon, Vector2 frameScale, Vector2 iconScale) : base(position, frame, icon, frameScale, iconScale)
        {
            Ability = ability;
            Index = index;
            IsButton = true;
        }

        public override void OnClick()
        {
            base.OnClick();
            Ability.actor.SelectAbility(Index);
        }
    }
}