using Kintsugi.AI;
using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Await;
using Kintsugi.EventSystem.Events;
using TacticsGameTest.Abilities;

namespace TacticsGameTest.Units
{
    internal class EnemyActor : CombatActor
    {
        public EnemyActor(string name, string spritePath, CharacterStats stats) : base(name, spritePath, stats)
        {
            team = 1;
            pathfindingSettings.SetCostLayer("constraint", float.PositiveInfinity, 10000);
            pathfindingSettings.SetCostLayer("spike", float.PositiveInfinity, 10000);
            pathfindingSettings.CheckAgainstTriggers = true;

        }
    }
    internal class BasicAttackEnemy : EnemyActor
    {
        public bool isRanged;
        public BasicAttackEnemy(string name, string spritePath, CharacterStats stats, List<Vec2Int> attackPattern) : base(name, spritePath, stats)
        {
            MeleeAttack = new BasicAttack(this, attackPattern);
            Move = new Stride(this);
            ((Stride)Move).AllowOnlySingleMove = true;

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
            ((BasicAttack)MeleeAttack).RangedSound = isRanged;
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

                        MoveTowardsClosest();

                        return true;
                    }
                    return true;
                }
            }

            return false;

        }

        public void MoveTowardsClosest()
        {
            PathfindingResult longLook = PathfindingSystem.Dijkstra(
                Transform.Grid,
                Transform.Position,
                100,
                pathfindingSettings);

            Vec2Int ClosestDesired = Vec2Int.Zero;
            float closestCost = float.PositiveInfinity;
            foreach (var item in longLook.ReachablePositions())
            {
                if (CanAttackAt(item))
                {
                    var cost = longLook.GetCost(item);
                    if (cost < closestCost)
                    {
                        ClosestDesired = item;
                        closestCost = cost;

                    }

                }
            }

            var pathToDesired = longLook.PathTo(ClosestDesired);
            if (pathToDesired == null)
            {
                movesLeft = 0;
                EventManager.I.Queue(CheckEndTurn);
                return;
            }
            for (int i = pathToDesired.PathPositions.Count - 1; i >= 0; i--)
            {
                var target = pathToDesired.PathPositions[i];
                if (longLook.GetCost(target) <= stats.Swift)
                {
                    Move.Hover(target);
                    Move.DoAction(target);
                    Move.OnDeselect();
                    return;
                }
            }

        }

        public override void OnStartTurn()
        {
            if (Dead)
            {
                EndTurn();
                return;
            }
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



        }
    }
    internal class BasicRangedEnemy : BasicAttackEnemy
    {
        public static List<Vec2Int> GetAttackRange()
        {
            var list = new List<Vec2Int>();
            for (int x = -4; x <= 4; x++)
            {
                for (int y = -4; y <= 4; y++)
                {
                    if (!(x >= -1 && x <= 1 && y >= -1 && y <= 1))
                    {
                        list.Add(new Vec2Int(x, y));
                    }
                }
            }
            return list;
        }
        public BasicRangedEnemy(string name, string spritePath, CharacterStats stats) : base(name, spritePath, stats, GetAttackRange())
        {
            isRanged = true;
        }

    }
    internal class BasicMeleeEnemy : BasicAttackEnemy
    {
        public static List<Vec2Int> GetAttackRange()
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

        public BasicMeleeEnemy(string name, string spritePath, CharacterStats stats) : base(name, spritePath, stats, GetAttackRange())
        {
        }


    }

}
