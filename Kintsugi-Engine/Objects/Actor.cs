namespace Kintsugi.Objects
{
    /// <summary>
    /// A <see cref="TileObject"/> that acts as a member in a control group.
    /// </summary>
    public abstract class Actor : TileObject
    {
        /// <summary>
        /// Whether this actor currently is in an active turn.
        /// </summary>
        public bool InTurn { get; private set; } = false;
        /// <summary>
        /// Called after this actors turn ends, just after OnTurnEnd.
        /// </summary>
        public event EventHandler OnActorTurnEnd;

        /// <summary>
        /// Called on the start of this actors turn, after this actors control group start turn.
        /// </summary>
        public abstract void OnStartTurn();

        /// <summary>
        /// Called on the end of this actors turn, just before this actors control group end turn.
        /// </summary>
        public abstract void OnEndTurn();

        /// <summary>
        /// Called on the start of the round, just after this actors control group start round.
        /// </summary>
        public abstract void OnStartRound();

        /// <summary>
        /// Called on the end of the round, just before this actors control group end round.
        /// </summary>
        public abstract void OnEndRound();

        internal void StartRound()
        {
            OnStartRound();
        }
        internal void EndRound()
        {
            OnEndRound();
        }
        internal void StartTurn()
        {
            OnStartTurn();
            InTurn = true;
        }
        internal void Update()
        {

        }

        /// <summary>
        /// End this actors turn. Will automatically end the control groups turn, if all actors have ended their turn.
        /// </summary>
        public void EndTurn()
        {
            InTurn = false;
            OnEndTurn();
            OnActorTurnEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}
