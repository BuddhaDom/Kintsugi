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
using System.Drawing;
using Kintsugi.Objects.Graphics;
using Kintsugi.EventSystem.Await;
using Kintsugi.UI;

namespace TacticsGameTest
{
    internal class SelectableActor : Actor, IInputListener
    {
        private PathfindingSettings pathfindingSettings = new();
        public string spritePath;
        private PathfindingResult PathfindingResult;
        public string name;
        private int maxMoves = 2;
        private int movesLeft;
        public int MovementRange = 5;
        public Canvas ActorUI = new();
        public SelectableActor(string name, string spritePath)
        {
            this.name = name;
            this.spritePath = spritePath;
            SetCharacterAnimation(AnimationDirection.right, AnimationType.idle, 1f);
            SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.5);
            SetCollider(["unit"], ["water", "wall", "unit"]);
            pathfindingSettings.AddCollideLayers(Collider.CollideLayers);
            pathfindingSettings.SetCostLayer("road", 0.5f, 1);
            pathfindingSettings.SetCostLayer("shrubbery", 2f, 1);
            pathfindingSettings.SetCostLayer("unit", float.PositiveInfinity, 100);

            Bootstrap.GetInput().AddListener(this);

            SetHealthUI();
        }
        public int healthMax = 5;
        public int health = 3;
        private List<CanvasObject> healthUI = new();
        private void SetHealthUI()
        {
            for (int i = 0; i < healthMax; i++)
            {
                if (!(i < healthUI.Count))
                {
                    var newObject = new CanvasObject();
                    newObject.FollowedTileobject = this;
                    ActorUI.Objects.Add(newObject);
                    healthUI.Add(newObject);
                    newObject.TargetPivot = new Vector2(0.25f, -0.5f);
                }
                SetHeartAnimation(i);
            }

            void SetHeartAnimation(int index)
            {

                healthUI[index].SetAnimation(
                    Bootstrap.GetAssetManager().GetAssetPath("PixelHearts\\hearts.png"),
                    16,
                    16,
                    13,
                    1f,
                    Enumerable.Range(27, 13),
                    default,
                    new Vector2(8, 8));
                healthUI[index].Graphic.Scale = new Vector2(4, 4);
            }
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
        private void AddHighlight(Vec2Int pos, Color color)
        {
            color = Color.FromArgb((byte)(0.65 * 256), color);
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
            walkHighlights.Add(mark);
        }
        private bool _isSelected;
        public void Select()
        {
            _isSelected = true;
            BeginPathfind();
            foreach (var position in GetAttackPositions())
            {
                if (GetActorIfAttackable(position) != null)
                {
                    AddHighlight(position, Color.OrangeRed);
                }
            }
            Console.WriteLine("im selected!!");
        }
        public SelectableActor GetActorIfAttackable(Vec2Int position)
        {
            if (GetAttackPositions().Contains(position))
            {
                var targeted = Transform.Grid.GetObjectsAtPosition(position);
                if (targeted == null) return null;
                foreach (var item in targeted)
                {
                    if (item is SelectableActor a)
                    {
                        return a;
                    }
                }

            }
            return null;
        }
        public void BeginPathfind()
        {
            if (movesLeft == 0)
            {
                Console.WriteLine("no moves left but trying to pathfind");
                return;
            }

            PathfindingResult = PathfindingSystem.Dijkstra(
                Transform.Grid,
                Transform.Position,
                MovementRange * movesLeft,
                pathfindingSettings);
            foreach (var item in PathfindingResult.ReachablePositions())
            {
                if (item != PathfindingResult.StartPosition)
                {
                    AddHighlight(item, IsSprint(PathfindingResult.GetCost(item)) ? Color.NavajoWhite : Color.Aqua);
                }
            }

        }
        private void WalkAlongCurrentPath()
        {
            movesLeft--;
            if (IsSprint(PathfindingResult.GetCost(path.PathPositions.Last())))
            {
                movesLeft--;
            }
            Event curEvent = new DummyEvent();
            EventManager.I.Queue(curEvent);
            foreach (var item in path.PathPositions.Skip(1))
            {
                curEvent = new ActionEvent(() => MoveTo(item))
                    .AddFinishAwait(this.Easing)
                    .AddStartAwait(curEvent);
                EventManager.I.Queue(curEvent);
            }
            var lastEvent = new ActionEvent(() => SetCharacterAnimation(null, AnimationType.idle, 1f))
                .AddStartAwait(curEvent);
            EventManager.I.Queue(lastEvent);
            EventManager.I.Queue(new ActionEvent(CheckEndTurn).AddStartAwait(lastEvent));
            Unselect();

        }
        private bool IsSprint(float cost)
        {
            return cost > MovementRange;
        }
        public bool IsAttackPosition(Vec2Int pos)
        {
            return GetAttackPositions().Contains(pos);
        }
        public List<Vec2Int> GetAttackPositions()
        {
            return new List<Vec2Int>()
            {
                Transform.Position + Vec2Int.Down,
                Transform.Position + Vec2Int.Up,
                Transform.Position + Vec2Int.Left,
                Transform.Position + Vec2Int.Right
            };
        }
        public void Unselect()
        {
            PathfindingResult = null;
            _isSelected = false;
            RemoveWalkHighlights();
            ClearPath();
            Console.WriteLine("im not selected :(");
        }
        public override void OnEndRound()
        {
            Console.WriteLine(name + " End Round");
        }

        public override void OnEndTurn()
        {
            Graphic.Modulation = Color.FromArgb(64, 64, 64);
            Console.WriteLine(name + " End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine(name + " Start Round");
        }

        public override void OnStartTurn()
        {
            movesLeft = maxMoves;
            Console.WriteLine(name + " Start Turn");
        }
        public void CheckEndTurn()
        {
            if (movesLeft == 0)
            {
                EndTurn();
            }
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
                        pathSegment.AddToGrid(Transform.Grid, 3);
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

        public void HandleInput(InputEvent inp, string eventType)
        {
            if (InTurn && _isSelected)
            {
                if (eventType == "MouseMotion")
                {
                    var gridPos = Transform.Grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                    SetPath(gridPos);
                }
                if (eventType == "MouseDown")
                {
                    if (inp.Button == SDL.SDL_BUTTON_LEFT)
                    {
                        var gridPos = Transform.Grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));

                        if (path != null)
                        {
                            WalkAlongCurrentPath();
                        }
                        else if (IsAttackPosition(gridPos))
                        {
                            var target = GetActorIfAttackable(gridPos);
                            if (target != null)
                            {
                                Attack(target);
                            }
                        }
                        else
                        {
                            Unselect();
                        }
                    }
                    else if (inp.Button == SDL.SDL_BUTTON_RIGHT)
                    {
                        Unselect();
                    }

                }

            }

        }

        private AnimationDirection AnimationDirectionToTarget(Vec2Int from, Vec2Int to)
        {
            Vec2Int dir = to - Transform.Position;
            AnimationDirection animationDirection;
            if (Math.Abs(dir.x) > Math.Abs(dir.y))
            {
                if (Math.Sign(dir.x) > 0)
                {
                    animationDirection = AnimationDirection.right;
                }
                else
                {
                    animationDirection = AnimationDirection.left;
                }
            }
            else
            {
                if (Math.Sign(dir.y) > 0)
                {
                    animationDirection = AnimationDirection.down;
                }
                else
                {
                    animationDirection = AnimationDirection.up;
                }
            }

            return animationDirection;
        }

        public void MoveTo(Vec2Int to)
        {
            AnimationDirection animDirection = AnimationDirectionToTarget(Transform.Position, to);
            Vec2Int dir = to - Transform.Position;

            float speed = MathF.Max(pathfindingSettings.GetCost(to, Transform.Grid), 0.1f);


            SetCharacterAnimation(animDirection, AnimationType.walk, speed);
            SetPosition(to);


        }

        public enum AnimationType { idle, walk, attack, death }
        public enum AnimationDirection { left, right, up, down }
        private AnimationType curAnimationType;
        private AnimationDirection curAnimationDirection;

        public void SetCharacterAnimation(AnimationDirection? dir, AnimationType? type, float speed)
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
                0.5 * speed,
                Enumerable.Range(0, 4),
                new Vector2(0.5f, 0.5f),
                new Vector2(16, 16),
                default,
                new Vector2(0, directionSection * 32 * 5 + typeSection * 32),
                type == AnimationType.attack ? 1 : 0);

            SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, speed * 0.5f);
            curAnimationDirection = dir.Value;
            curAnimationType = type.Value;

            if (dir == AnimationDirection.left)
            {
                Graphic.Flipped = true;
            }
            else
            {
                Graphic.Flipped = false;
            }


        }

        public void Attack(SelectableActor target)
        {
            Unselect();
            movesLeft--;
            var animationDirection = AnimationDirectionToTarget(Transform.Position, target.Transform.Position);

            var beginAttack = new ActionEvent(() =>
            {
                SetCharacterAnimation(
                    AnimationDirectionToTarget(Transform.Position, target.Transform.Position),
                    AnimationType.attack,
                    1f);
            });

            var spawnHitEffect = new ActionEvent(() =>
            {
                var hitEffect = new TileObject();
                hitEffect.AddToGrid(target.Transform.Grid, target.Transform.Layer);
                hitEffect.SetPosition(target.Transform.Position, false);
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
            EventManager.I.Queue(new ActionEvent(CheckEndTurn).AddStartAwaits([beginAttack, spawnHitEffect]));
        }
    }
}
