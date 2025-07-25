﻿using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Events;

namespace Kintsugi.Objects
{
    /// <summary>
    /// An abstract controller for rounds.
    /// </summary>
    public abstract class ControlGroup
    {
        /// <summary>
        /// Called when an actor ends their turn, just after control group turn end.
        /// </summary>
        public event EventHandler ControlGroupTurnEnd;
        /// <summary>
        /// Whether this control group has finished their turn this round .
        /// </summary>
        public bool HasHadTurn { get; internal set; }
        public abstract float CalculateInitiative();

        /// <summary>
        /// Called on the start of round, just before this groups actors start round calls.
        /// </summary>
        public abstract void OnStartRound();
        /// <summary>
        /// Called on the end of round, just after this groups actors end round calls.
        /// </summary>
        public abstract void OnEndRound();
        /// <summary>
        /// Called once when the control group starts their turn, called just before the groups actors start their turns.
        /// </summary>
        public abstract void OnStartTurn();
        /// <summary>
        /// Called once when the control group ends their turn, called just after the last actor ends their turn.
        /// </summary>
        public abstract void OnEndTurn();



        /// <summary>
        /// Add an actor to this control group.
        /// </summary>
        /// <param name="actor">Actor to be added.</param>
        /// <exception cref="Exception">If actor is null.</exception>
        public void AddActor(Actor actor)
        {
            if (actor == null) throw new Exception("Cannot add null actor to group");
            actors.Add(actor);
        }
        /// <summary>
        /// Remove an actor from the control group.
        /// </summary>
        /// <param name="actor">Actor to be removed.</param>
        public void RemoveActor(Actor actor)
        {
            actors.Remove(actor);
        }
        /// <summary>
        /// Get a list of all actors in the control group.
        /// </summary>
        /// <param name="actor">Actor to be removed.</param>
        public IEnumerable<Actor> GetActors() 
        {
            return actors;
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

        public float CurrentInitiative { get; set; }
        internal void RecalculateInitiative()
        {
            CurrentInitiative = CalculateInitiative();
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
