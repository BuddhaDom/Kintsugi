using Kintsugi.AI;
using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static TacticsGameTest.Units.CombatActor;

namespace TacticsGameTest.Units
{
    internal class AnimatableActor :
        Actor
    {
        public enum AnimationType { idle, walk, attack, death }
        public enum AnimationDirection { left, right, up, down }

        public AnimatableActor(string path)
        {
            spritePath = path;
            SetCharacterAnimation(AnimationDirection.right, AnimationType.idle, 1f);
            SetEasing(TweenSharp.Animation.Easing.QuadraticEaseOut, 0.5);
        }
        public string spritePath;
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
        }
        AnimationDirection curAnimationDirection;
        AnimationType curAnimationType;
        public void MoveTo(Vec2Int to)
        {
            Audio.I.PlayAudio("Step");

            AnimationDirection animDirection = AnimationDirectionToTarget(Transform.Position, to);
            Vec2Int dir = to - Transform.Position;

            float speed = GetMoveSpeed(to);


            SetCharacterAnimation(animDirection, AnimationType.walk, speed);
            SetPosition(to);
        }
        public virtual float GetMoveSpeed(Vec2Int pos)
        {
            return 0.5f;
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
    }
}
