using Engine.EventSystem;
using Kintsugi.Core;
using Kintsugi.EventSystem.Await;
using Kintsugi.EventSystem.Events;
using Kintsugi.Objects.Graphics;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Units;
using static TacticsGameTest.Units.CombatActor;

namespace TacticsGameTest.Abilities
{
    internal class BasicAttack : Ability
    {
        public List<Vec2Int> attacks; 
        public BasicAttack(CombatActor actor, List<Vec2Int> attacks) : base(actor)
        {
            this.attacks = attacks;
        }
        public List<Vec2Int> GetAttackPositions()
        {
            return attacks.Select((a) => a + actor.Transform.Position).ToList();
        }
        public CombatActor GetActorIfAttackable(Vec2Int position)
        {
            if (GetAttackPositions().Contains(position))
            {
                var targeted = actor.Transform.Grid.GetObjectsAtPosition(position);
                if (targeted == null) return null;
                foreach (var item in targeted)
                {
                    if (item is CombatActor a)
                    {
                        return a;
                    }
                }

            }
            return null;
        }

        public override string Path => throw new NotImplementedException();

        public override string Title => throw new NotImplementedException();

        public override string Tooltip => throw new NotImplementedException();

        public override void DoAction(Vec2Int target)
        {
            var targetActor = GetActorIfAttackable(target);
            actor.movesLeft--;
            var animationDirection = actor.AnimationDirectionToTarget(actor.Transform.Position, targetActor.Transform.Position);

            var beginAttack = new ActionEvent(() =>
            {
                actor.SetCharacterAnimation(
                    actor.AnimationDirectionToTarget(actor.Transform.Position, targetActor.Transform.Position),
                    AnimationType.attack,
                    1f);
            });

            var spawnHitEffect = new ActionEvent(() =>
            {
                targetActor.TakeDamage(1, 1);
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

            }).AddStartAwait(new WaitForSeconds(0.25f));

            EventManager.I.Queue(beginAttack);
            EventManager.I.Queue(spawnHitEffect);
            EventManager.I.Queue(new ActionEvent(actor.CheckEndTurn).AddStartAwaits([beginAttack, spawnHitEffect]));
        }

        public override IEnumerable<(Vec2Int, Color)> GetTargets(Vec2Int from)
        {
            List<(Vec2Int, Color)> targets = new();
            foreach (var attackPos in GetAttackPositions())
            {
                if (GetActorIfAttackable(attackPos) != null)
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
