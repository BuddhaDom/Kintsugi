using Kintsugi.Core;
using System.Numerics;
using Kintsugi.AI;
using Kintsugi.EventSystem;
using Kintsugi.UI;
using TacticsGameTest.UI;

namespace TacticsGameTest.Units
{
    internal abstract class CombatActor : BaseUnit
    {
        public int tempHealth;
        public int team;
        public PathfindingSettings pathfindingSettings = new();
        public string name;
        public int movesLeft;
        public Canvas ActorUI = new();
        public CombatActor(string name, string spritePath, CharacterStats stats): base(spritePath)
        {
            this.stats = stats;
            this.name = name;
            SetCollider(["unit"], ["spike", "wall", "unit"]);
            pathfindingSettings.AddCollideLayers(Collider.CollideLayers);
            pathfindingSettings.SetCostLayer("road", 0.5f, 1);
            pathfindingSettings.SetCostLayer("shrubbery", 2f, 1);
            pathfindingSettings.SetCostLayer("unit", float.PositiveInfinity, 100);



            SetHealthUI();
        }
        public int poison;
        public float spacing = 16f;
        private List<Heart> healthUI = new();
        public void ApplyPoison()
        {
            stats.Hp -= poison;
            SetHealthUI();
            if (stats.Hp <= 0)
            {
                EventManager.I.QueueImmediate(() => Die());
            }
        }
        public void GainShield(int amnt)
        {
            tempHealth += amnt;
            SetHealthUI();
        }
        public void TakeDamage(int damage, int poison)
        {
            for (int i = 0; i < damage; i++)
            {
                if (tempHealth > 0)
                {
                    tempHealth--;
                }
                else
                {
                    stats.Hp--;
                }
            }
            this.poison += poison;
            SetHealthUI();
            if (stats.Hp <= 0)
            {
                EventManager.I.QueueImmediate(() => Die());
            }
        }


        int prevHealth;
        private void SetHealthUI()
        {
            for (int i = 0; i < stats.MaxHp + tempHealth; i++)
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
            for (int i = healthUI.Count - 1; i >= 0; i--)
            {
                if (i >= stats.MaxHp + tempHealth)
                {
                    var heart = healthUI[i];
                    ActorUI.Objects.Remove(heart);
                    healthUI.RemoveAt(i);

                }

            }

            for (int i = 0; i < healthUI.Count; i++)
            {
                healthUI[i].Position =
                    new Vector2((i - (healthUI.Count - 1) / 2f) * spacing, 0);
                if (i + poison < stats.Hp)
                {
                    healthUI[i].SetHeartAnimation(Heart.HeartMode.normal);
                }
                else if (i < stats.Hp)
                {
                    healthUI[i].SetHeartAnimation(Heart.HeartMode.poison);
                }
                else if (i >= stats.MaxHp)
                {
                    healthUI[i].SetHeartAnimation(Heart.HeartMode.armor);
/*
                    int diff = i - stats.MaxHp + 2;
                    if (tempHealth <= diff)
                    {
                        healthUI[i].SetHeartAnimation(Heart.HeartMode.armor);
                    }
                    else
                    {
                        healthUI[i].SetHeartAnimation(Heart.HeartMode.notinitialized);
                    }*/
                }
                else
                {
                    healthUI[i].SetHeartAnimation(Heart.HeartMode.gone);
                }

            }



            prevHealth = stats.Hp;
        }
        public override void OnEndRound()
        {
            Console.WriteLine(name + " End Round");
        }

        public override void OnEndTurn()
        {
            ApplyPoison();
            SetHealthUI();
            Console.WriteLine(name + " End Turn");
        }

        public override void OnStartRound()
        {
            Console.WriteLine(name + " Start Round");
        }

        public override void OnStartTurn()
        {
            tempHealth = 0;
            SetHealthUI();
            if (Dead)
            {
                EndTurn();
                return;
            }
            movesLeft = stats.MaxMoves;
            Console.WriteLine(name + " Start Turn");
        }
        public void CheckEndTurn()
        {
            if (InTurn && movesLeft == 0)
            {
                EndTurn();
            }
        }


        public void PushTo(Vec2Int to)
        {
            SetPosition(to);


        }
        public override float GetMoveSpeed(Vec2Int to)
        {
            return MathF.Max(pathfindingSettings.GetCost(to, Transform.Grid), 0.1f);

        }

        public bool Dead { get; private set; }
        public void Die()
        {
            Audio.I.PlayAudio("Death");
            SetCharacterAnimation(null, AnimatableActor.AnimationType.death, 1f);
            ActorUI.Visible = false;
            Dead = true;
            if (InTurn) EndTurn();

        }


    }
}
