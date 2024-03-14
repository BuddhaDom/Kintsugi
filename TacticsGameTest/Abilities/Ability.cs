using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest.Abilities
{
    internal abstract class Ability
    {
        public abstract IEnumerable<(Vec2Int, Color)> GetTargets(SelectableActor act, Vec2Int from);
        public abstract void Hover(Vec2Int target);
        public abstract void DoAction(Vec2Int target);
    }
}
