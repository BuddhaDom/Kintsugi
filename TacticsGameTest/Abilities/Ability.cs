using Kintsugi.Core;
using System.Drawing;
using TacticsGameTest.Units;

namespace TacticsGameTest.Abilities
{
    internal abstract class Ability
    {
        public CombatActor actor;
        public Ability(CombatActor actor)
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
