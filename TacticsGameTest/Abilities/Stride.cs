using Engine.EventSystem;
using Kintsugi.AI;
using Kintsugi.Core;
using Kintsugi.EventSystem.Events;
using Kintsugi.EventSystem;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest.Abilities
{
    internal class Stride : Ability
    {
        public PathfindingResult PathfindingResult;
        private Kintsugi.AI.Path path;

        public override string Path => throw new NotImplementedException();

        public override string Title => throw new NotImplementedException();

        public override string Tooltip => throw new NotImplementedException();

        public Stride(SelectableActor actor) : base(actor)
        {
        }
        private bool IsSprint(float cost)
        {
            return cost > actor.MovementRange;
        }

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
            var lastEvent = new ActionEvent(() => actor.SetCharacterAnimation(null, SelectableActor.AnimationType.idle, 1f))
                .AddStartAwait(curEvent);
            EventManager.I.Queue(lastEvent);
            EventManager.I.Queue(new ActionEvent(actor.CheckEndTurn).AddStartAwait(lastEvent));
            actor.Unselect();

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
                actor.MovementRange * actor.movesLeft,
                actor.pathfindingSettings);


            List<(Vec2Int, Color c)> highlights = new();

            foreach (var item in PathfindingResult.ReachablePositions())
            {
                if (item != PathfindingResult.StartPosition)
                {
                    highlights.Add((item, PathfindingResult.GetCost(item) > actor.MovementRange ? Color.NavajoWhite : Color.Aqua));
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
