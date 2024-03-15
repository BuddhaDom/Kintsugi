using Engine.EventSystem;
using FMOD;
using Kintsugi.Core;
using Kintsugi.EventSystem.Await;
using Kintsugi.EventSystem.Events;
using Kintsugi.Objects.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Abilities;

namespace TacticsGameTest.Units
{
    internal class EnemyActor : CombatActor
    {
        public EnemyActor(string name, string spritePath) : base(name, spritePath)
        {
            team = 1;
        }
    }
    internal class BasicMeleeEnemy : EnemyActor
    {
        public BasicMeleeEnemy(string name, string spritePath) : base(name, spritePath)
        {
            var attackPattern = new List<Vec2Int>()
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

            MeleeAttack = new BasicAttack(this, attackPattern);
            Move = new Stride(this);
        }
        Ability MeleeAttack;
        Ability Move;
        public PlayerActor SelectTarget;
        public Vec2Int ChooseTarget(List<Vec2Int> targets)
        {

            return targets[Dice.RandomRange(0, targets.Count)];
        }
        public bool TryAttack()
        {
            var targets = MeleeAttack.GetTargets(Transform.Position).Select((a) => a.Item1).ToList();
            if (targets.Count() > 0)
            {
                MeleeAttack.DoAction(ChooseTarget(targets));
                return true;
            }
            return false;
        }
        public bool CanAttackAt(Vec2Int pos)
        {
            var targets = MeleeAttack.GetTargets(pos).Select((a) => a.Item1).ToList();
            if (targets.Count() > 0)
            {
                return true;
            }
            return false;
        }
        public bool TryMove()
        {
            var asdjsdkf = Move.GetTargets(Transform.Position);
            if (asdjsdkf != null)
            {
                var targets = asdjsdkf.Select((a) => a.Item1).ToList();
                if (targets.Count() > 0)
                {
                    List<Vec2Int> validMoves = new();
                    foreach (var item in targets)
                    {
                        if (CanAttackAt(item))
                        {
                            validMoves.Add(item);
                        }
                    }
                    if (validMoves.Count > 0)
                    {
                        var chosenMove = ChooseTarget(validMoves);
                        Move.Hover(chosenMove);
                        Move.DoAction(chosenMove);
                        Move.OnDeselect();
                    }
                    else
                    {
                        return false;
                    }
                    return true;
                }
            }

            return false;

        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();


            var newEvent = new ActionEvent(() =>
            {
                if (!TryAttack())
                {
                    TryMove();
                }

            });
            var event2 = new ActionEvent(() =>
            {
                if (!TryAttack())
                {
                    TryMove();
                }

            }).AddStartAwait((IAwaitable)MeleeAttack).AddStartAwait((IAwaitable)Move);

            EventManager.I.Queue(newEvent);
            EventManager.I.Queue(event2);

            EventManager.I.Queue(EndTurn);


        }

    }

}
