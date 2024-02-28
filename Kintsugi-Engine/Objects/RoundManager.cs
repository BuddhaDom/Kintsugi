using Kintsugi.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Objects
{
    public class RoundManager
    {
        public event EventHandler OnRoundFinished;

        public ControlGroup? CurrentControlGroup
        {
            get
            {
                if (currentControlGroup < 0 || currentControlGroup >= controlGroups.Count)
                {
                    return null;
                }
                return controlGroups[currentControlGroup];
            }
        }
        public RoundManager(ScenarioManager scenarioManager) {
            this.scenarioManager = scenarioManager;
        }

        internal void AddToTurnOrder(ControlGroup c)
        {
            c.RecalculateInitiative();
            controlGroups.Add(c);
            Sort();
        }

        internal void Begin()
        {
            RedoTurnOrder();
            foreach (var controlGroup in controlGroups)
            {
                controlGroup.StartRound();
            }
            NextTurn();
        }

        private void NextTurn()
        {

            currentControlGroup++;
            if (ValidGroup())
            {
                CurrentControlGroup.StartTurn();
                CurrentControlGroup.ControlGroupTurnEnd += HandleControlGroupTurnEnd;
            }
            else
            {
                foreach (var controlGroup in controlGroups)
                {
                    controlGroup.EndRound();
                }
                OnRoundFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandleControlGroupTurnEnd(object? sender, EventArgs e)
        {
            CurrentControlGroup.ControlGroupTurnEnd -= HandleControlGroupTurnEnd;
            NextTurn();
        }

        private void RedoTurnOrder()
        {
            if(scenarioManager.RecalculateInitiativeOnNewRound){
                foreach (var controlGroup in controlGroups)
                {
                    controlGroup.RecalculateInitiative();
                }
            }
            currentControlGroup = -1;

            Sort();
        }

        private void Sort()
        {
            controlGroups.Sort(ControlGroupComparer);
        }

        private static int ControlGroupComparer(ControlGroup a, ControlGroup b)
        {
            if (a.CurrentInitiative == b.CurrentInitiative)
            {
                return 0;
            }
            else if (a.CurrentInitiative < b.CurrentInitiative)
            {
                return 1;
            }
            else// (a.currentInitiative > b.currentInitiative)
            {
                return -1;
            }
        }
        private bool ValidGroup()
        {
            return currentControlGroup >= 0 && currentControlGroup < controlGroups.Count;
        }

        private int currentControlGroup = -1;

        List<ControlGroup> controlGroups = new();
        private ScenarioManager scenarioManager;
    }

}
