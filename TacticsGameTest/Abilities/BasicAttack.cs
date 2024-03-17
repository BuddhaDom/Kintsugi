using Kintsugi.Core;
using Kintsugi.EventSystem.Await;
using Kintsugi.EventSystem.Events;
using Kintsugi.Objects.Graphics;
using Kintsugi.Objects;
using System.Drawing;
using System.Numerics;
using TacticsGameTest.Units;
using Kintsugi.EventSystem;
using TacticsGameTest.UI;
using System.Runtime.InteropServices.Marshalling;

namespace TacticsGameTest.Abilities
{
    internal class DaggerThrow : BasicAttack
    {
        public override string Path => UIHelper.Get(16);
        public override string Title => "Throwing dagger";
        public override string Tooltip => "Damage 1D2 with a range of 3!";

        private static List<Vec2Int> GetAttacks()
        {
            return new List<Vec2Int>()
            {
                new Vec2Int(0, 3),

                new Vec2Int(-1, 2),
                new Vec2Int(0,  2),
                new Vec2Int(1,  2),

                new Vec2Int(-2, 1),
                new Vec2Int(-1, 1),
                new Vec2Int(0,  1),
                new Vec2Int(1,  1),
                new Vec2Int(2,  1),

                new Vec2Int(-3, 0),
                new Vec2Int(-2, 0),
                new Vec2Int(-1, 0),
                new Vec2Int(0,  0),
                new Vec2Int(1,  0),
                new Vec2Int(2,  0),
                new Vec2Int(3, 0),

                new Vec2Int(-2, -1),
                new Vec2Int(-1, -1),
                new Vec2Int(0,  -1),
                new Vec2Int(1,  -1),
                new Vec2Int(2,  -1),

                new Vec2Int(-1, -2),
                new Vec2Int(0,  -2),
                new Vec2Int(1,  -2),

                new Vec2Int(0, -3),
            };

        }
        public DaggerThrow(CombatActor actor) : base(actor, GetAttacks())
        {
        }
        public override void OnHit(CombatActor targetActor)
        {
            Audio.I.PlayAudio("RangedAttack");
            targetActor.TakeDamage(Dice.Roll(actor.stats.DamageRangedAmount, actor.stats.DamageRangedType), 0);
        }
    }
    internal class PoisonDagger : BasicAttack
    {
        public override string Path => UIHelper.Get(3);
        public override string Title => "Poisonous Stab";
        public override string Tooltip => "1D2 damage and 1D2 poison!";

        private static List<Vec2Int> GetAttacks()
        {
            return new List<Vec2Int>()
            {
                new Vec2Int(-1, -1),
                new Vec2Int(-1, 0),
                new Vec2Int(-1, 1),
                new Vec2Int(0, -1),
                new Vec2Int(0, 1),
                new Vec2Int(1, -1),
                new Vec2Int(1, 0),
                new Vec2Int(1, 1),
            };

        }
        public PoisonDagger(CombatActor actor) : base(actor, GetAttacks())
        {
        }
        public override void OnHit(CombatActor targetActor)
        {
            targetActor.TakeDamage(Dice.Roll(actor.stats.DamageMeleeAmount, actor.stats.DamageMeleeType), Dice.Roll(actor.stats.DamageMeleeAmount, actor.stats.DamageMeleeType));
        }
    }
    internal class SpearStab : BasicAttack
    {
        public override string Path => UIHelper.Get(23);
        public override string Title => "Spear Stab";
        public override string Tooltip => "Range of two. 1D4 damage!";

        private static List<Vec2Int> GetAttacks()
        {
            return new List<Vec2Int>()
            {
                new Vec2Int(-1, -1),
                new Vec2Int(-1, -1) * 2,

                new Vec2Int(-1, 0),
                new Vec2Int(-1, 0) * 2,

                new Vec2Int(-1, 1),
                new Vec2Int(-1, 1) * 2,

                new Vec2Int(0, -1),
                new Vec2Int(0, -1) * 2,

                new Vec2Int(0, 1),
                new Vec2Int(0, 1) * 2,

                new Vec2Int(1, -1),
                new Vec2Int(1, -1) * 2,

                new Vec2Int(1, 0),
                new Vec2Int(1, 0) * 2,

                new Vec2Int(1, 1),
                new Vec2Int(1, 1) * 2,

            };

        }
        public SpearStab(CombatActor actor) : base(actor, GetAttacks())
        {
        }
        public override void OnHit(CombatActor targetActor)
        {
            Audio.I.PlayAudio("MeleeAttack");
            targetActor.TakeDamage(Dice.Roll(actor.stats.DamageMeleeAmount, actor.stats.DamageMeleeType), 0);
        }
    }

    internal class AxeSwing : BasicAttack
    {
        public override string Path => base.Path;
        public override string Title => "Axe swing.";
        public override string Tooltip => "1D2 Damage";

        private static List<Vec2Int> GetAttacks()
        {
            return new List<Vec2Int>()
            {
                new Vec2Int(-1, -1),
                new Vec2Int(-1, 0),
                new Vec2Int(-1, 1),
                new Vec2Int(0, -1),
                new Vec2Int(0, 1),
                new Vec2Int(1, -1),
                new Vec2Int(1, 0),
                new Vec2Int(1, 1),
            };

        }
        public AxeSwing(CombatActor actor) : base(actor, GetAttacks())
        {
        }
        public override void OnHit(CombatActor targetActor)
        {
            Audio.I.PlayAudio("MeleeAttack");
            targetActor.TakeDamage(Dice.Roll(actor.stats.DamageMeleeAmount, actor.stats.DamageMeleeType), 0);
        }
    }

    internal class BasicAttack : Ability, IAwaitable
    {
        public bool RangedSound;
        public override string Path => UIHelper.Get(29);

        public override string Title => "Attack";

        public override string Tooltip => "Damage with your WEAPON dice!";

        public List<Vec2Int> attacks; 
        public BasicAttack(CombatActor actor, List<Vec2Int> attacks) : base(actor)
        {
            this.attacks = attacks;
        }
        public List<Vec2Int> GetAttackPositions(Vec2Int position)
        {
            return attacks.Select((a) => a + position).ToList();
        }
        public CombatActor GetActorIfAttackable(Vec2Int from, Vec2Int to)
        {
            if (GetAttackPositions(from).Contains(to))
            {
                var targeted = actor.Transform.Grid.GetObjectsAtPosition(to);
                if (targeted == null) return null;
                foreach (var item in targeted)
                {
                    if (item is CombatActor a && a.team != actor.team && !a.Dead)
                    {
                        return a;
                    }
                }

            }
            return null;
        }
        public virtual void OnHit(CombatActor targetActor)
        {
            if (RangedSound)
            {
                Audio.I.PlayAudio("RangedAttack");
            }
            else
            {
                Audio.I.PlayAudio("MeleeAttack");
            }
            targetActor.TakeDamage(Dice.Roll(actor.stats.DamageMeleeAmount, actor.stats.DamageMeleeType), 0);

        }

        public override void DoAction(Vec2Int target)
        {
            var targetActor = GetActorIfAttackable(actor.Transform.Position, target);
            var animationDirection = actor.AnimationDirectionToTarget(actor.Transform.Position, targetActor.Transform.Position);

            var beginAttack = new ActionEvent(() =>
            {
                actor.SetCharacterAnimation(
                    actor.AnimationDirectionToTarget(actor.Transform.Position, targetActor.Transform.Position),
                    AnimatableActor.AnimationType.attack,
                    1f);
            });

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

                var removeEffect = new ActionEvent(() =>
                {
                    hitEffect.RemoveFromGrid();
                }).AddStartAwait(((Animation)hitEffect.Graphic));
                EventManager.I.Queue(removeEffect);
                hitEffect.Graphic.Modulation = Color.Red;

            }).AddStartAwait(new WaitForSeconds(0.25f));
            spawnHitEffect.SetAsQueueBlocker();
            EventManager.I.Queue(beginAttack);
            EventManager.I.Queue(spawnHitEffect);
            lastevent = new ActionEvent(actor.CheckEndTurn).AddStartAwaits([beginAttack, spawnHitEffect]);
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
            foreach (var attackPos in GetAttackPositions(from))
            {
                if (GetActorIfAttackable(from, attackPos) != null)
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
