using Engine.EventSystem;
using Kintsugi.EventSystem.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Objects
{
    public abstract class ControlGroup
    {
        public event EventHandler ControlGroupTurnEnd;
        public bool HasHadTurn { get; internal set; }
        public abstract float CalculateInitiative();
        public abstract void OnStartRound();
        public abstract void OnEndRound();

        public abstract void OnStartTurn();
        public abstract void OnEndTurn();

        public void SetInitiative(float value)
        {
            currentInitiative = value;
        }

        public void AddActor(Actor actor)
        {
            actors.Add(actor);
        }
        public void RemoveActor(Actor actor)
        {
            actors.Remove(actor);
        }


        internal void StartRound()
        {
            HasHadTurn = false;
            OnStartRound();
            foreach (var unit in actors)
            {
                unit.StartRound();
            }
        }
        internal void EndRound()
        {
            foreach (var unit in actors)
            {
                unit.EndRound();
            }
            OnEndRound();
        }

        internal float currentInitiative;
        internal void RecalculateInitiative()
        {
            currentInitiative = CalculateInitiative();
        }
        private int awaitingActors = 0;
        internal void StartTurn()
        {
            awaitingActors = actors.Count;
            foreach (var unit in actors)
            // technically we need to do this first in an extreme edge case
            // where the dev ends everyones turn immediately...
            {
                unit.OnActorTurnEnd += UnitTurnOver;
            }
            OnStartTurn();
            foreach (var unit in actors)
            {
                unit.StartTurn();
            }
        }
        internal void EndTurn()
        {
            OnEndTurn();
            HasHadTurn = true;
            ControlGroupTurnEnd?.Invoke(this, EventArgs.Empty);
        }

        private void UnitTurnOver(object? sender, EventArgs e)
        {
            if (sender == null)
            {
                throw new NullReferenceException("Null actor ended its turn, which is illegal.");
            }

            var actor = sender as Actor;
            actor.OnActorTurnEnd -= UnitTurnOver;
            awaitingActors--;

            if (awaitingActors == 0)
            {
                EventManager.I.Queue(new EndTurnEvent(this));
            }
        }

        private List<Actor> actors = new();

    }

}
