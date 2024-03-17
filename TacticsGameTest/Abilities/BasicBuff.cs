using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Await;
using Kintsugi.EventSystem.Events;
using Kintsugi.Objects;
using Kintsugi.Objects.Graphics;
using System.Drawing;
using System.Numerics;
using TacticsGameTest.UI;
using TacticsGameTest.Units;

namespace TacticsGameTest.Abilities
{

    internal class BasicBuff : Ability, IAwaitable
    {
        public override string Path => UIHelper.Get(26);

        public override string Title => "Buff";

        public override string Tooltip => "Give temp health = intuition!";

        public List<Vec2Int> attacks; 
        public BasicBuff(CombatActor actor, List<Vec2Int> attacks) : base(actor)
        {
            this.attacks = attacks;
        }
        public List<Vec2Int> GetBuffPositions(Vec2Int position)
        {
            return attacks.Select((a) => a + position).ToList();
        }
        public CombatActor GetActorIfBuffable(Vec2Int from, Vec2Int to)
        {
            if (GetBuffPositions(from).Contains(to))
            {
                var targeted = actor.Transform.Grid.GetObjectsAtPosition(to);
                if (targeted == null) return null;
                foreach (var item in targeted)
                {
                    if (item is CombatActor a && a.team == actor.team)
                    {
                        return a;
                    }
                }

            }
            return null;
        }
        public virtual void OnHit(CombatActor targetActor)
        {
            Audio.I.PlayAudio("Inspire");
            targetActor.GainShield(actor.stats.Intuition);
            
            //targetActor.TakeDamage(1, 0);

        }

        public override void DoAction(Vec2Int target)
        {
            var targetActor = GetActorIfBuffable(actor.Transform.Position, target);
            var animationDirection = actor.AnimationDirectionToTarget(actor.Transform.Position, targetActor.Transform.Position);


            var spawnHitEffect = new ActionEvent(() =>
            {
                actor.movesLeft--;

                OnHit(targetActor);
                var hitEffect = new TileObject();
                hitEffect.AddToGrid(targetActor.Transform.Grid, targetActor.Transform.Layer);
                hitEffect.SetPosition(targetActor.Transform.Position, false);
                hitEffect.SetAnimation(
                    Bootstrap.GetAssetManager().GetAssetPath("FantasyBattlePack\\CriticalHit.png"),
                    32,
                    32,
                    4,
                    0.5f,
                    Enumerable.Range(0, 4),
                    new Vector2(-0.5f, -0.5f),
                    default,
                    default,
                    default,
                    1);
                hitEffect.Graphic.Modulation = Color.Green;

                var removeEffect = new ActionEvent(() =>
                {
                    hitEffect.RemoveFromGrid();
                }).AddStartAwait(((Animation)hitEffect.Graphic));
                EventManager.I.Queue(removeEffect);

            });
            spawnHitEffect.SetAsQueueBlocker();
            EventManager.I.Queue(spawnHitEffect);
            lastevent = new ActionEvent(actor.CheckEndTurn).AddStartAwaits([spawnHitEffect]);
            EventManager.I.Queue(lastevent);
        }
        private Event lastevent;
        public bool IsFinished()
        {
            return lastevent?.IsFinished() ?? true;
        }

        public override IEnumerable<(Vec2Int, Color)> GetTargets(Vec2Int from)
        {
            List<(Vec2Int, Color)> targets = new();
            foreach (var attackPos in GetBuffPositions(from))
            {
                if (GetActorIfBuffable(from, attackPos) != null)
                {
                    targets.Add((attackPos, Color.OrangeRed));
                }
            }
            return targets;
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
