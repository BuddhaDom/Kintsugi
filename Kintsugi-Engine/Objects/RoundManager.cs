namespace Kintsugi.Objects
{
    /// <summary>
    /// A controller for things that happen in a round. A container for control groups.
    /// </summary>
    public class RoundManager
    {
        /// <summary>
        /// Handler for round events.
        /// </summary>
        public event EventHandler OnRoundFinished;

        /// <summary>
        /// Get the control group whose turn it currently is.
        /// </summary>
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
        /// <summary>
        /// Create a round instance on a given scenario.
        /// </summary>
        /// <param name="scenarioManager">Scenario to which this round belongs to.</param>
        public RoundManager(ScenarioManager scenarioManager) {
            this.scenarioManager = scenarioManager;
        }

        /// <summary>
        /// Add a control group to the turn order, automatically places it according to its initiative.
        /// </summary>
        internal void AddToTurnOrder(ControlGroup c)
        {
            c.RecalculateInitiative();
            controlGroups.Add(c);
            Sort();
        }

        internal void Begin()
        {
            RedoTurnOrder();
            scenarioManager.OnBeginRound();
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
                scenarioManager.OnBeginTurn();
                CurrentControlGroup.StartTurn();
                CurrentControlGroup.ControlGroupTurnEnd += HandleControlGroupTurnEnd;
            }
            else
            {
                foreach (var controlGroup in controlGroups)
                {
                    controlGroup.EndRound();
                }
                scenarioManager.OnEndRound();
                OnRoundFinished?.Invoke(this, EventArgs.Empty);
            }
        }

        private void HandleControlGroupTurnEnd(object? sender, EventArgs e)
        {
            CurrentControlGroup.ControlGroupTurnEnd -= HandleControlGroupTurnEnd;
            scenarioManager.OnEndTurn();
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
