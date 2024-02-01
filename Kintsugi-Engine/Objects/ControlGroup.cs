using Kintsugi_Engine.Core;
using Kintsugi_Engine.Objects;
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
        public abstract float GetInitiative();
        public abstract void OnStartRound();
        public abstract void OnEndRound();

        public abstract void OnStartTurn();
        public abstract void OnEndTurn();

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
            currentInitiative = GetInitiative();
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

            var unit = sender as Unit;
            unit.OnUnitTurnEnd -= UnitTurnOver;
            awaitingUnits--;

            if (awaitingUnits == 0)
            {
                EndTurn();
            }
        }

        private List<Unit> units = new();

    }

}
