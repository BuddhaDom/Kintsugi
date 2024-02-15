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

        public void AddActor(Actor actor)
        {
            units.Add(actor);
        }
        public void RemoveActor(Actor actor)
        {
            units.Remove(actor);
        }


        internal void StartRound()
        {
            HasHadTurn = false;
            OnStartRound();
            foreach (var unit in units)
            {
                unit.StartRound();
            }
        }
        internal void EndRound()
        {
            foreach (var unit in units)
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
        private int awaitingUnits = 0;
        internal void StartTurn()
        {
            awaitingUnits = units.Count;
            foreach (var unit in units)
            // technically we need to do this first in an extreme edge case
            // where the dev ends everyones turn immediately...
            {
                unit.OnUnitTurnEnd += UnitTurnOver;
            }
            OnStartTurn();
            foreach (var unit in units)
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
                throw new NullReferenceException("Null unit ended its turn, which is illegal.");
            }

            var unit = sender as Actor;
            unit.OnUnitTurnEnd -= UnitTurnOver;
            awaitingUnits--;

            if (awaitingUnits == 0)
            {
                EventManager.I.Queue(new EndTurnEvent(this));
            }
        }

        private List<Actor> units = new();

    }

}
