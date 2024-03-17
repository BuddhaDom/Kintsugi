using Engine.EventSystem;
using Kintsugi.Core;
using Kintsugi.EventSystem.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.UI;
using TacticsGameTest.Units;

namespace TacticsGameTest.Abilities
{
    internal class Guard : Ability
    {
        public Guard(CombatActor actor) : base(actor)
        {
        }

        public override string Path => UIHelper.Get(9);

        public override string Title => "Guard";

        public override string Tooltip => "Gain temporary health that lasts till next round.";

        public override void DoAction(Vec2Int target)
        {
            actor.GainShield(2);

            actor.movesLeft--;
            var endturn =  new ActionEvent(actor.CheckEndTurn);
            EventManager.I.Queue(endturn);
        }

        public override IEnumerable<(Vec2Int, Color)> GetTargets(Vec2Int from)
        {
            return [(actor.Transform.Position, Color.SkyBlue)];
        }

        public override void Hover(Vec2Int target)
        {
        }

        public override void OnDeselect()
        {
        }

        public override void OnSelect()
        {
        }
    }
}
