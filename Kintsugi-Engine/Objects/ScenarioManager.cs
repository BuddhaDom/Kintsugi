namespace Kintsugi.Objects
{
    /// <summary>
    /// A manager for a scenario. A collection of control groups.
    /// </summary>
    public abstract class ScenarioManager
    {
        /// <summary>
        /// Invoked when the scenario is over.
        /// </summary>
        public event EventHandler OnTurnOrderFinished;
        private bool ended;
        public ScenarioManager()
        {
            roundManager = new(this);
        }

        private bool begun = false;
        /// <summary>
        /// Whether to recalculate initiative before sorting the turn order each round.
        /// </summary>
        public bool RecalculateInitiativeOnNewRound = false;
        /// <summary>
        /// If the scenario ends suddenly, should the current round and turn end-hooks trigger?
        /// </summary>
        public bool ResolveCurrentRoundOnScenarioStop = false; // Does not work yet

        /// <summary>
        /// Called at the beginning of the scenario.
        /// </summary>
        public abstract void OnBeginScenario();
        /// <summary>
        /// Called at the end of the scenario.
        /// </summary
        public abstract void OnEndScenario();
        /// <summary>
        /// Called at the beginning of every round, before any other OnBeginRound in the round.
        /// </summary
        public abstract void OnBeginRound();
        /// <summary>
        /// Called at the end of every round, after any other OnEndRound in the round.
        /// </summary
        public abstract void OnEndRound();
        /// <summary>
        /// Called at the beginning of every actor turn, before any other OnBeginTurn in the turn.
        /// </summary
        public abstract void OnBeginTurn();
        /// <summary>
        /// Called at the end of every actor turn, before any other OnEndTurn in the turn.
        /// </summary
        public abstract void OnEndTurn();

        /// <summary>
        /// Begin the scenario, starting the automatic turn system.
        /// </summary>
        /// <exception cref="InvalidOperationException">When scenario has already started.</exception>
        public void BeginScenario()
        {
            if (begun)
            {
                throw new InvalidOperationException("Scenario has already begun!");
            }
            begun = true;
            OnBeginScenario();
            SetupAndBeginScenario();
        }

        /// <summary>
        /// End the scenario, stopping the automatic starting and ending of rounds and turns.
        /// See <see cref="ResolveCurrentRoundOnScenarioStop"/> for adjusting whether this should trigger hooks or not.
        /// </summary>
        public void EndScenario()
        {
            OnEndScenario();
            roundManager.OnRoundFinished -= NextRound;
            OnTurnOrderFinished?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Add a control group to the scenario and add them to the turn order.
        /// </summary>
        public void AddControlGroup(ControlGroup c)
        {
            if (c == null) throw new Exception("Cannot add null control group to scenario");

            roundManager.AddToTurnOrder(c);
        }
        internal void NextRound(object? sender, EventArgs e)
        {
            roundManager.Begin();
        }

        internal void SetupAndBeginScenario()
        {
            roundManager.OnRoundFinished += NextRound;
            roundManager.Begin();
        }

        private RoundManager roundManager;

    }
}
