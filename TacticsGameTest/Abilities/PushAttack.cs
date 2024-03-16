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
using static TacticsGameTest.Units.CombatActor;
using TacticsGameTest.Units;
using TacticsGameTest.Events;

namespace TacticsGameTest.Abilities
{
    internal class PushAttack : BasicAttack
    {
        public PushAttack(CombatActor actor, List<Vec2Int> attacks) : base(actor, attacks)
        {
        }

        public override void DoAction(Vec2Int target)
        {
            var targetActor = GetActorIfAttackable(actor.Transform.Position, target);
            actor.movesLeft--;
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
                Audio.I.PlayAudio("Shove");
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


                Vec2Int targetDir = targetActor.Transform.Position - actor.Transform.Position;

                var curAwaitEvent = removeEffect;
                for (int i = 0; i < 3; i++)
                {
                    var pushEvent = new PushedEvent(targetActor, targetDir)
                        .AddStartAwaits(curAwaitEvent)
                        .AddFinishAwait(targetActor.Easing);
                    curAwaitEvent = pushEvent;
                    EventManager.I.Queue(pushEvent);
                }
                var endturnCheck = new ActionEvent(actor.CheckEndTurn).AddStartAwaits([curAwaitEvent]);
                EventManager.I.Queue(endturnCheck);




            }).AddStartAwait(new WaitForSeconds(0.25f));

            EventManager.I.Queue(beginAttack);
            EventManager.I.Queue(spawnHitEffect);
        }
    }
}
