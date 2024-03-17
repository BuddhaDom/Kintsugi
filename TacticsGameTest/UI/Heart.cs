using Engine.EventSystem;
using Kintsugi.Core;
using Kintsugi.EventSystem.Events;
using Kintsugi.Objects.Graphics;
using Kintsugi.UI;
using System.Numerics;

namespace TacticsGameTest.UI
{
    internal class Heart : CanvasObject
    {
        public enum HeartMode { normal, gone, poisonbegin, poison, notinitialized, armor }
        private HeartMode previousMode;

        private int columns = 13;
        public void SetHeartAnimation(HeartMode mode)
        {
            if (previousMode == HeartMode.notinitialized)
            {
                SetAnimation(
                Bootstrap.GetAssetManager().GetAssetPath("PixelHearts\\hearts.png"),
                16,
                16,
                13,
                1f,
                Enumerable.Range(27, 13),
                default,
                new Vector2(8, 8),
                default,
                default,
                1);
            }
            else
            {
                switch (mode)
                {
                    case HeartMode.normal:
                        if (previousMode == HeartMode.notinitialized)
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 2, 13), 1);
                        }
                        else if (previousMode != HeartMode.normal)
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 2 + 9, 4), 1);
                        }
                        else
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 3, 1), 1);
                        }
                        break;
                    case HeartMode.gone:
                        if (previousMode != HeartMode.gone)
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 1 + 9, 3).Append(0), 1);
                        }
                        else
                        {
                            SetAnimationOverride(Enumerable.Range(0, 1), 1);
                        }
                        break;
                    case HeartMode.poisonbegin:
                        if (previousMode == HeartMode.normal)
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 15, 6), 1);
                        }
                        else
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 15 + 5, 1), 1);
                        }
                        break;
                    case HeartMode.poison:
                        if (previousMode == HeartMode.normal)
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 15, 6), 1);
                            var full = new ActionEvent(() => SetHeartAnimation(HeartMode.poison)).AddStartAwait((Animation)Graphic);
                            EventManager.I.QueueImmediate(full);
                        }
                        else
                        {
                            SetAnimationOverride(Enumerable.Range(columns * 16, 6), 0);
                        }
                        break;
                    case HeartMode.armor:
                        SetAnimationOverride(Enumerable.Range(columns * 6, 6), 1);
                        break;
                    case HeartMode.notinitialized:
                        SetAnimationOverride(Enumerable.Range(4, 1), 1);
                        break;
                }
                Graphic.Scale = new Vector2(4, 4);
                previousMode = mode;
            }

        }

        private void SetAnimationOverride(IEnumerable<int> frames, int repeats)
        {
            SetAnimation(
                Bootstrap.GetAssetManager().GetAssetPath("PixelHearts\\hearts.png"),
                16,
                16,
                13,
                1f,
                frames,
                default,
                new Vector2(8, 8),
                default,
                default,
                repeats);

        }
    }
}
