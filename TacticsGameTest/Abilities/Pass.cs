using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Events;
using System.Drawing;
using TacticsGameTest.UI;
using TacticsGameTest.Units;

namespace TacticsGameTest.Abilities
{
    internal class Pass : Ability
    {
        public Pass(CombatActor actor) : base(actor)
        {
        }

        public override string Path => UIHelper.Get(36);

        public override string Title => "Pass";

        public override string Tooltip => "Pass turn.";

        public override void DoAction(Vec2Int target)
        {
        }

        public override IEnumerable<(Vec2Int, Color)> GetTargets(Vec2Int from)
        {
            return [];
        }

        public override void Hover(Vec2Int target)
        {
        }

        public override void OnDeselect()
        {
        }

        public override void OnSelect()
        {
            actor.movesLeft = 0;

            var endturn = new ActionEvent(actor.CheckEndTurn);
            EventManager.I.Queue(endturn);

        }
    }
}
