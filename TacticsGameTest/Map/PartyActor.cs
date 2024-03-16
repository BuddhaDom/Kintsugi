using Engine.EventSystem;
using Kintsugi.AI;
using Kintsugi.Core;
using Kintsugi.EventSystem.Events;
using Kintsugi.EventSystem;
using Kintsugi.Input;
using Kintsugi.Objects;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Abilities;
using TacticsGameTest.Events;
using TacticsGameTest.Units;
using System.Drawing;
using Kintsugi.Collision;

namespace TacticsGameTest.Map
{
    internal class PartyActor : AnimatableActor, IInputListener
    {
        public override void OnEndRound()
        {
        }

        public override void OnEndTurn()
        {
        }

        public override void OnStartRound()
        {
        }

        public override void OnStartTurn()
        {
            StartPathing();
        }
        private Kintsugi.AI.Path path;

        public PartyActor(string path) : base(path)
        {
            pathfindingSettings = new();
            pathfindingSettings.SetDefaultCost(float.PositiveInfinity);
            pathfindingSettings.SetCostLayer("ground", 0, -100);
            pathfindingSettings.SetCostLayer("room", 1);

            Bootstrap.GetInput().AddListener(this);
        }

        public void Move()
        {
            Event curEvent = new DummyEvent();
            EventManager.I.Queue(curEvent);
            foreach (var item in path.PathPositions.Skip(1))
            {
                curEvent = new ActionEvent(() => MoveTo(item))
                    .AddFinishAwait(Easing)
                    .AddStartAwait(curEvent);
                EventManager.I.Queue(curEvent);
            }
            var lastEvent = new ActionEvent(() => SetCharacterAnimation(null, CombatActor.AnimationType.idle, 1f))
                .AddStartAwait(curEvent);
            EventManager.I.Queue(lastEvent);
            var startNewPath = new ActionEvent(() => StartPathing()).AddStartAwait(lastEvent);
            EventManager.I.Queue(startNewPath);

            EventManager.I.Queue(new ActionEvent(CheckEndTurn).AddStartAwait(lastEvent));

        }
        public void CheckEndTurn()
        {
            MapManagement.I.EnterRoom(Transform.Position);
            EndTurn();
        }
        public PathfindingSettings pathfindingSettings;
        public PathfindingResult PathfindingResult;
        public void StartPathing()
        {
            ClearHighlights();
            ClearPath();
            PathfindingResult = PathfindingSystem.Dijkstra(
                Transform.Grid,
                Transform.Position,
                1,
                pathfindingSettings);


            List<Vec2Int> highlights = new();

            foreach (var item in GetReachableRooms())
            {
                if (item != PathfindingResult.StartPosition)
                {
                    AddHighlight(item);
                }
            }

        }
        public List<Vec2Int> GetReachableRooms()
        {
            var collider = new Collider();
            collider.CollideLayers.Add("room");
            var reachable = new List<Vec2Int>();
            foreach (var item in PathfindingResult.ReachablePositions())
            {
                if (CollisionSystem.CollidesColliderWithGridAtPosition(collider, Transform.Grid, item))
                {
                    reachable.Add(item);
                }
            }
            return reachable;
        }
        private void ClearHighlights()
        {
            foreach (var item in highlights)
            {
                item.RemoveFromGrid();
            }
            highlights.Clear();
        }

        private List<TileObject> highlights = new();
        private void AddHighlight(Vec2Int pos)
        {
            var color = Color.FromArgb((byte)(0.65 * 256), Color.White);
            var mark = new TileObject();
            mark.AddToGrid(Transform.Grid, 3);
            mark.SetAnimation(
                Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath("TinyBattles\\mark-sheet.png"),
                16,
                16,
                4,
                0.5f,
                Enumerable.Range(0, 4));
            mark.Graphic.Modulation = color;
            mark.SetEasing(TweenSharp.Animation.Easing.BounceEaseOut, 0.5f);
            mark.SetPosition(Transform.Position, false);
            mark.SetPosition(pos);
            highlights.Add(mark);
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

        public void PathTo(Vec2Int target)
        {
            if (path != null)
            {
                if (path.PathPositions.Last() == target)
                {
                    return;

                }

            }
            if (!GetReachableRooms().Contains(target))
            {
                ClearPath();
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
                        pathSegment.AddToGrid(Transform.Grid, 4);
                        pathSegment.SetSpriteSingle(Bootstrap.GetRunningGame().GetAssetManager().GetAssetPath(PathToSpritePath(i)));
                        pathSegment.SetPosition(path.PathPositions[i]);
                        pathSegments.Add(pathSegment);

                    }
                }


            }
        }
        public void HandleInput(InputEvent inp, string eventType)
        {
            if (InTurn && EventManager.I.IsQueueDone())
            {
                if (eventType == "MouseMotion")
                {
                    var gridPos = Transform.Grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                    PathTo(gridPos);
                    //SetPath(gridPos);
                }
                if (eventType == "MouseDown")
                {
                    if (inp.Button == SDL.SDL_BUTTON_LEFT)
                    {
                        var gridPos = Transform.Grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                        if (path != null && PathfindingResult!= null && PathfindingResult.ReachablePositions().Contains(gridPos))
                        {
                            Move();
                        }
                    }
                }
            }
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
