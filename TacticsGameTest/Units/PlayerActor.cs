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
    internal class PlayerActor : CombatActor, IInputListener
    {
        public List<Ability> abilities;
        private Ability selectedAbility;
        public void SelectAbility(int index)
        {
            Ability newAbility = null;
            if (index < abilities.Count && index >= 0)
            {
                newAbility = abilities[index];
            }
            if (newAbility != selectedAbility)
            {
                if (selectedAbility != null)
                {
                    selectedAbility.OnDeselect();
                    targetPositions.Clear();
                    ClearHighlights();
                }
                if (newAbility != null)
                {
                    newAbility.OnSelect();
                    targetPositions.Clear();
                    foreach (var item in newAbility.GetTargets(Transform.Position))
                    {
                        AddHighlight(item.Item1, item.Item2);
                        targetPositions.Add(item.Item1);
                    }
                }
                selectedAbility = newAbility;
            }
        }
        private HashSet<Vec2Int> targetPositions = new();
        public void DeselectAbility()
        {
            SelectAbility(-1);
        }

        private List<TileObject> highlights = new();

        private void ClearHighlights()
        {
            foreach (var item in highlights)
            {
                item.RemoveFromGrid();
            }
            highlights.Clear();
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
            highlights.Add(mark);
        }
        private bool _isSelected;

        public PlayerActor(string name, string spritePath) : base(name, spritePath)
        {
            team = 0;
            abilities = new();
            abilities.Add(new Stride(this));
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

            abilities.Add(new BasicAttack(this, attackPattern));
            abilities.Add(new PushAttack(this, attackPattern));
        }

        private bool justSelected = false;
        public void Select()
        {
            _isSelected = true;
            justSelected = true;
            SelectAbility(0);
            /*
            foreach (var position in GetAttackPositions())
            {
                if (GetActorIfAttackable(position) != null)
                {
                    AddHighlight(position, Color.OrangeRed);
                }
            }
            */
            Console.WriteLine("im selected!!");
            HUD.Instance.DisplayActor(this);
        }
        public void Unselect()
        {
            _isSelected = false;
            ClearHighlights();
            DeselectAbility();
            Console.WriteLine("im not selected :(");
            HUD.Instance.Clear();
        }
        public override void OnEndRound()
        {
            base.OnEndRound();
        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();
            Graphic.Modulation = Color.FromArgb(64, 64, 64);
        }

        public override void OnStartRound()
        {
            base.OnStartRound();
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();
        }

        public void HandleInput(InputEvent inp, string eventType)
        {
            if (justSelected)
            {
                justSelected = false;
                return;
            }
            if (InTurn && _isSelected && !Dead)
            {
                if (eventType == "KeyDown")
                {

                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_1)
                    {
                        SelectAbility(0);
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_2)
                    {
                        SelectAbility(1);
                    }
                    if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_3)
                    {
                        SelectAbility(2);
                    }
                }
                if (eventType == "MouseMotion")
                {
                    var gridPos = Transform.Grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                    selectedAbility?.Hover(gridPos);
                    //SetPath(gridPos);
                }
                if (eventType == "MouseDown")
                {
                    if (inp.Button == SDL.SDL_BUTTON_LEFT)
                    {
                        var gridPos = Transform.Grid.WorldToGridPosition(Bootstrap.GetCameraSystem().ScreenToWorldSpace(new Vector2(inp.X, inp.Y)));
                        if (targetPositions.Contains(gridPos))
                        {
                            selectedAbility?.DoAction(gridPos);
                            if (movesLeft <= 0)
                            {
                                Unselect();
                            }
                            DeselectAbility();
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
    }
}
