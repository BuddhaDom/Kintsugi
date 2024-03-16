using System.Numerics;
using Kintsugi.Core;
using Kintsugi.Objects.Graphics;
using Kintsugi.UI;
using TacticsGameTest.Abilities;

namespace TacticsGameTest.UI;

internal class AbilityFrame : IHUDObject
{
    private AbilityFramedIcon Icon { get; }

    private AbilityTooltipFrame Tooltip { get; }

    public AbilityFrame(Ability ability, int index, Vector2 position)
    {
        Tooltip = new AbilityTooltipFrame(
            ability.Title, ability.Tooltip,
            new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("GUI\\abilityDescription.png")),
            position+ new Vector2(100f, 5f)
        );
        
        Icon = new AbilityFramedIcon(
            ability, index, position, Tooltip,
            new SpriteSingle(Bootstrap.GetAssetManager().GetAssetPath("GUI\\abilityFrame.png")),
            new SpriteSingle(ability.Path),
            Vector2.One * 5, Vector2.One * 2.25f
        );
    }

    public void AddToCanvas(Canvas canvas)
    {
        Icon.AddToCanvas(canvas);
        Tooltip.AddToCanvas(canvas);
    }

    private class AbilityFramedIcon : FramedIcon
    {
        private Ability Ability { get; }
        private int Index { get; }
        private AbilityTooltipFrame TooltipFrame { get; }
        
        public AbilityFramedIcon(Ability ability,
            int index,
            Vector2 position,
            AbilityTooltipFrame tooltip,
            ISpriteable frame,
            ISpriteable icon,
            Vector2 frameScale,
            Vector2 iconScale) : base(position,
            frame,
            icon,
            frameScale,
            iconScale)
        {
            IsButton = true;
            
            Ability = ability;
            Index = index;
            TooltipFrame = tooltip;
            TooltipFrame.Visible = false;
        }

        public override void OnClick()
        {
            base.OnClick();
            Ability.actor.SelectAbility(Index);
        }

        public override void OnHoverStart()
        {
            base.OnHoverStart();
            TooltipFrame.Visible = true;
            TooltipFrame.Title.Visible = true;
            TooltipFrame.Description.Visible = true;
        }

        public override void OnHoverEnd()
        {
            base.OnHoverEnd();
            TooltipFrame.Visible = false;
            TooltipFrame.Title.Visible = false;
            TooltipFrame.Description.Visible = false;
        }
    }
}