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
using TacticsGameTest.Abilities;
using TacticsGameTest.UI;

namespace TacticsGameTest.Units
{
    internal abstract class CombatActor : BaseUnit
    {
        public int team;
        public PathfindingSettings pathfindingSettings = new();
        public string spritePath;
        public string name;
        private int maxMoves = 2;
        public int movesLeft;
        public int MovementRange = 5;
        public Canvas ActorUI = new();
        public CombatActor(string name, string spritePath)
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



            SetHealthUI();
        }
        public int healthMax = 5;
        public int health = 3;
        public int poison;
        public float spacing = 16f;
        private List<Heart> healthUI = new();
        public void TakeDamage(int damage, int poison)
        {
            health -= damage;
            this.poison += poison;
            SetHealthUI();
            if (health <= 0)
            {
                EventManager.I.QueueImmediate(() => Die());
            }
        }


        int prevHealth;
        private void SetHealthUI()
        {
            for (int i = 0; i < healthMax; i++)
            {
                if (!(i < healthUI.Count))
                {
                    var newObject = new Heart();
                    newObject.FollowedTileobject = this;
                    ActorUI.Objects.Add(newObject);
                    healthUI.Add(newObject);
                    newObject.TargetPivot = new Vector2(0.25f, -0.25f);
                }
            }

            for (int i = 0; i < healthUI.Count; i++)
            {
                healthUI[i].Position =
                    new Vector2((i - (healthUI.Count - 1) / 2f) * spacing, 0);
                if (i + poison < health)
                {
                    healthUI[i].SetHeartAnimation(Heart.HeartMode.normal);
                }
                else if (i < health)
                {
                    healthUI[i].SetHeartAnimation(Heart.HeartMode.poison);
                }
                else
                {
                    healthUI[i].SetHeartAnimation(Heart.HeartMode.gone);
                }

            }



            prevHealth = health;
        }
        public override void OnEndRound()
        {
            Console.WriteLine(name + " End Round");
        }

        public override void OnEndTurn()
        {
            TakeDamage(poison, 0);
            SetHealthUI();
            Console.WriteLine(name + " End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine(name + " Start Round");
        }

        public override void OnStartTurn()
        {
            if (Dead)
            {
                EndTurn();
                return;
            }
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

        public AnimationDirection AnimationDirectionToTarget(Vec2Int from, Vec2Int to)
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

        public void PushTo(Vec2Int to)
        {
            SetPosition(to);


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
                type == AnimationType.attack || type == AnimationType.death ? 1 : 0);

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
        public bool Dead { get; private set; }
        public void Die()
        {
            SetCharacterAnimation(null, AnimationType.death, 1f);
            ActorUI.Visible = false;
            Dead = true;
            if (InTurn) EndTurn();

        }


    }
}
