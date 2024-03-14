using Kintsugi.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest.Abilities
{
    internal class Stride : Ability
    {
        public override void DoAction(Vec2Int target)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<(Vec2Int, Color)> GetTargets(SelectableActor act, Vec2Int from)
        {
            throw new NotImplementedException();
        }

        public override void Hover(Vec2Int target)
        {
            throw new NotImplementedException();
        }
    }
}
