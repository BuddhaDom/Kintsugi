using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Objects;
using Kintsugi.EventSystem;
using SDL2;
using Engine.EventSystem;
using Kintsugi.EventSystem.Events;
using Kintsugi.Tiles;
using System.Numerics;
using Kintsugi.AI;

namespace TacticsGameTest
{
    internal class SelectableActor : Actor, IInputListener
    {
        public string spritePath;
        private PathfindingResult PathfindingResult;
        public string name;
        public SelectableActor(string name, string spritePath)
        {
            this.name = name;
            this.spritePath = spritePath;
            SetCharacterAnimation(AnimationDirection.right, AnimationType.idle);
            SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.5);

            Bootstrap.GetInput().AddListener(this);
        }
        private List<TileObject> walkHighlights = new();
        private List<TileObject> pathSegments = new();

        private void RemoveWalkHighlights()
        {
            foreach (var item in walkHighlights)
            {
                item.RemoveFromGrid();
            }
            walkHighlights.Clear();
        }
        private void AddWalkHighlight(Vec2Int pos)
        {
            var mark = new TileObject();
            mark.AddToGrid(Transform.Grid, 0);
            mark.SetSpriteSingle(Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath("TinyBattles\\mark.png"));
            mark.SetEasing(TweenSharp.Animation.Easing.BounceEaseOut, 0.5f);
            mark.SetPosition(Transform.Position, false);
            mark.SetPosition(pos);
            walkHighlights.Add(mark);
        }
        private bool _isSelected;
        public void Select()
        {
            _isSelected = true;
            PathfindingResult = PathfindingSystem.Dijkstra(
                Transform.Grid,
                Transform.Position,
                5);
            foreach (var item in PathfindingResult.ReachablePositions())
            {
                if (item != PathfindingResult.StartPosition)
                {
                    AddWalkHighlight(item);
                }
            }
            Console.WriteLine("im selected!!");
        }
        public void Unselect()
        {
            _isSelected = false;
            PathfindingResult = null;
            RemoveWalkHighlights();
            Console.WriteLine("im not selected :(");
        }
        public override void OnEndRound()
        {
            Console.WriteLine(name + " End Round");
        }

        public override void OnEndTurn()
        {
            Console.WriteLine(name + " End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine(name + " Start Round");
        }

        public override void OnStartTurn()
        {

            Console.WriteLine(name + " Start Turn");
        }

        public void SetPath(Vec2Int pos)
        {
            if (path != null && path.PathPositions.Last() == pos)
            {
                return;
            }
            ClearPath();
            if (PathfindingResult != null)
            {
                path = PathfindingResult.PathTo(pos);
                if (path != null)
                {
                    for (int i = 0; i < path.PathPositions.Count; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        var pathSegment = new TileObject();
                        pathSegment.AddToGrid(Transform.Grid, 0);
                        pathSegment.SetSpriteSingle(Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath(PathToSpritePath(i)));
                        pathSegment.SetPosition(path.PathPositions[i]);
                        pathSegments.Add(pathSegment);

                    }
                }


            }
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
        private Kintsugi.AI.Path path;
        private string PathToSpritePath(int index)
        {
            string imagePath = "TinyBattles\\arrow";
            List<string> chars = new();
            if (index == 0)
            {
                chars.Add(DiffToChar(path.PathPositions[index], path.PathPositions[index + 1]));
            }
            else if (index ==  path.PathPositions.Count - 1)
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

        public void HandleInput(InputEvent inp, string eventType)
        {
            if (eventType == "MouseMotion")
            {
                var gridPos = Transform.Grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                SetPath(gridPos);
            }
            if (eventType == "MouseDown")
            {
                if (path != null)
                {
                    Event curEvent = new DummyEvent();
                    EventManager.I.Queue(curEvent);
                    foreach (var item in path.PathPositions.Skip(1))
                    {
                        curEvent = new ActionEvent(() => MoveTo(item))
                            .AddFinishAwait(this.Easing)
                            .AddStartAwait(curEvent);
                        EventManager.I.Queue(curEvent);
                    }
                    EventManager.I.Queue(new ActionEvent(() => SetCharacterAnimation(null, AnimationType.idle))
                        .AddStartAwait(curEvent));
                    ClearPath();
                    RemoveWalkHighlights();
                    PathfindingResult = null;
                }
            }

        }

        public void MoveTo(Vec2Int to)
        {
            AnimationDirection animDirection;
            Vec2Int dir = to - Transform.Position;
            if (Math.Abs(dir.x) > Math.Abs(dir.y))
            {
                if (Math.Sign(dir.x) > 0)
                {
                    animDirection = AnimationDirection.left;
                }
                else
                {
                    animDirection = AnimationDirection.right;
                }
            }
            else
            {
                if (Math.Sign(dir.y) > 0)
                {
                    animDirection = AnimationDirection.down;
                }
                else
                {
                    animDirection = AnimationDirection.up;
                }
            }

            SetCharacterAnimation(animDirection, AnimationType.walk);


            SetPosition(to);

        }

        public enum AnimationType { idle, walk, death, attack }
        public enum AnimationDirection { left, right, up, down }
        private AnimationType curAnimationType;
        private AnimationDirection curAnimationDirection;

        public void SetCharacterAnimation(AnimationDirection? dir, AnimationType? type)
        {
            if (dir == null) dir = curAnimationDirection;
            if (type == null) type = curAnimationType;

            var fullPath = Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath(spritePath);

            var directionSection = 0;
            if (dir == AnimationDirection.down)
            {
                directionSection = 1;
            }
            if (dir == AnimationDirection.up)
            {
                directionSection = 2;
            }
            var typeSection = (int)type;

            SetAnimation(
                fullPath,
                32,
                32,
                4,
                0.5,
                Enumerable.Range(0, 4),
                new Vector2(-0.5f, -0.5f),
                default,
                default,
                new Vector2(0, directionSection * 32 * 5 + typeSection * 32));

            curAnimationDirection = dir.Value;
            curAnimationType = type.Value;
        }
    }
}
