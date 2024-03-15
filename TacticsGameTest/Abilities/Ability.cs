using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Units;

namespace TacticsGameTest.Abilities
{
    internal abstract class Ability
    {
        public SelectableActor actor;
        public Ability(SelectableActor actor)
        {
            this.actor = actor;
        }
        public abstract void OnSelect();
        public abstract void OnDeselect();
        public abstract IEnumerable<(Vec2Int, Color)> GetTargets(Vec2Int from);
        public abstract void Hover(Vec2Int target);
        public abstract void DoAction(Vec2Int target);
        public abstract string Path { get; }
        public abstract string Title { get; }
        public abstract string Tooltip { get; }

    }
}
