using Engine.EventSystem;
using Kintsugi.AI;
using Kintsugi.Core;
using Kintsugi.EventSystem.Events;
using Kintsugi.EventSystem;
using Kintsugi.Objects;
using System.Drawing;
using TacticsGameTest.Units;
using TacticsGameTest.Events;
using Kintsugi.EventSystem.Await;
using TacticsGameTest.UI;

namespace TacticsGameTest.Abilities
{
    internal class Stride : Ability, IAwaitable
    {
        public PathfindingResult PathfindingResult;
        private Kintsugi.AI.Path path;
        public bool AllowOnlySingleMove;

        public override string Path => UIHelper.Get(19);

        public override string Title => "Stride";

        public override string Tooltip => "Move up to your SPEED!";

        public Stride(CombatActor actor) : base(actor)
        {
        }
        private bool IsSprint(float cost)
        {
            return cost > actor.Swift;
        }

        public bool IsFinished()
        {
            return waitingEvent?.IsFinished() ?? true;
        }
        private IAwaitable waitingEvent;
        public override void DoAction(Vec2Int target)
        {
            actor.movesLeft--;
            if (IsSprint(PathfindingResult.GetCost(path.PathPositions.Last())))
            {
                actor.movesLeft--;
            }
            Event curEvent = new DummyEvent();
            EventManager.I.Queue(curEvent);
            foreach (var item in path.PathPositions.Skip(1))
            {
                curEvent = new ActionEvent(() => actor.MoveTo(item))
                    .AddFinishAwait(actor.Easing)
                    .AddStartAwait(curEvent);
                EventManager.I.Queue(curEvent);
            }
            var lastEvent = new ActionEvent(() => actor.SetCharacterAnimation(null, CombatActor.AnimationType.idle, 1f))
                .AddStartAwait(curEvent);
            waitingEvent = lastEvent;
            EventManager.I.Queue(lastEvent);
            EventManager.I.Queue(new ActionEvent(actor.CheckEndTurn).AddStartAwait(lastEvent));

        }

        public override IEnumerable<(Vec2Int, Color)> GetTargets(Vec2Int from)
        {
            if (actor.movesLeft == 0)
            {
                Console.WriteLine("no moves left but trying to pathfind");
                return null;
            }

            PathfindingResult = PathfindingSystem.Dijkstra(
                actor.Transform.Grid,
                actor.Transform.Position,
                AllowOnlySingleMove ? actor.Swift : actor.Swift * actor.movesLeft,
                actor.pathfindingSettings);


            List<(Vec2Int, Color c)> highlights = new();

            foreach (var item in PathfindingResult.ReachablePositions())
            {
                if (item != PathfindingResult.StartPosition)
                {
                    highlights.Add((item, PathfindingResult.GetCost(item) > actor.Swift ? Color.NavajoWhite : Color.Aqua));
                }
            }

            return highlights;
        }

        public override void Hover(Vec2Int target)
        {
            if (path != null && path.PathPositions.Last() == target)
            {
                return;
            }
            ClearPath();
            if (PathfindingResult != null)
            {
                path = PathfindingResult.PathTo(target);
                if (path != null)
                {
                    for (int i = 0; i < path.PathPositions.Count; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        var pathSegment = new TileObject();
                        pathSegment.AddToGrid(actor.Transform.Grid, 3);
                        pathSegment.SetSpriteSingle(Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath(PathToSpritePath(i)));
                        pathSegment.SetPosition(path.PathPositions[i]);
                        pathSegments.Add(pathSegment);

                    }
                }


            }
        }

        public override void OnDeselect()
        {
            PathfindingResult = null;
            ClearPath();

        }

        public override void OnSelect()
        {


        }
        public void ClearPath()
        {
            path = null;
            foreach (var item in pathSegments)
            {
                item.RemoveFromGrid();
            }
            pathSegments.Clear();

        }
        private List<TileObject> pathSegments = new();


        private string PathToSpritePath(int index)
        {
            string imagePath = "TinyBattles\\arrow";
            List<string> chars = new();
            if (index == 0)
            {
                chars.Add(DiffToChar(path.PathPositions[index], path.PathPositions[index + 1]));
            }
            else if (index == path.PathPositions.Count - 1)
            {
                chars.Add(DiffToChar(path.PathPositions[index], path.PathPositions[index - 1]));
            }
            else
            {
                chars.Add(DiffToChar(path.PathPositions[index], path.PathPositions[index + 1]));
                chars.Add(DiffToChar(path.PathPositions[index], path.PathPositions[index - 1]));
            }

            chars.Sort();

            foreach (var item in chars)
            {
                imagePath += item;
            }
            return imagePath + ".png";

            string DiffToChar(Vec2Int midPos, Vec2Int otherPos)
            {
                var diff = otherPos - midPos;
                if (diff.x == 1)
                {
                    return "L";
                }
                if (diff.x == -1)
                {
                    return "R";
                }
                if (diff.y == 1)
                {
                    return "D";
                }
                if (diff.y == -1)
                {
                    return "U";
                }
                return "";
            }
        }
    }
}
