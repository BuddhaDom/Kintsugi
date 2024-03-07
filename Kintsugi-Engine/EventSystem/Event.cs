using Kintsugi.EventSystem.Await;

namespace Kintsugi.EventSystem
{
    /// <summary>
    /// Base class for an event to be used in the event system.
    /// </summary>
    public abstract class Event: IAwaitable
    {
        private List<IAwaitable> _startAwaits;
        private List<IAwaitable> _finishAwaits;
        /// <summary>
        /// Has this event been executed already?
        /// Causes execute to throw an exception if true.
        /// </summary>
        public bool HasBeenExecuted { get; protected set; }
        /// <summary>
        /// Execute the event. Events can only executed once by default.
        /// </summary>
        public void Execute()
        {
            if (!HasBeenExecuted)
            {
                Console.WriteLine("Executing " + this.ToString() );
                OnExecute();
                HasBeenExecuted = true;
            }
            else
            {
                throw new Exception("Executed event that has already been executed: " + this);
            }
        }
        /// <summary>
        /// Code ran when event is executed. Events can only be executed once by default.
        /// </summary>
        public abstract void OnExecute();
        protected bool shouldBlockQueue;
        /// <summary>
        /// Whether this event should block any subsequent events from executing while not finished.
        /// </summary>
        public virtual bool BlockQueue () { return shouldBlockQueue; }

        /// <summary>
        /// Set as a blocker.
        /// </summary>
        public Event SetAsQueueBlocker()
        {
            shouldBlockQueue = true;
            return this;
        }

        /// <summary>
        /// Add an awaitable that must be finished before this event is finished.
        /// </summary>
        public Event AddFinishAwait(IAwaitable await)
        {
            _finishAwaits ??= new List<IAwaitable>();
            _finishAwaits.Add(await);
            return this;
        }

        /// <summary>
        /// Add awaitables that must be finished before this event is finished.
        /// </summary>
        public Event AddFinishAwait(params IAwaitable[] awaits)
        {
            _finishAwaits ??= new List<IAwaitable>();
            foreach (var await in awaits)
            {
                _finishAwaits.Add(await);
            }
            return this;
        }

        /// <summary>
        /// Add an awaitable that must be finished before this event can be executed.
        /// </summary>
        public Event AddStartAwait(IAwaitable await)
        {
            _startAwaits ??= new List<IAwaitable>();
            _startAwaits.Add(await);
            return this;
        }
        /// <summary>
        /// Add an awaitables that must be finished before this event can be executed.
        /// </summary>
        public Event AddStartAwaits(params IAwaitable[] awaits)
        {
            _startAwaits ??= new List<IAwaitable>();
            foreach (var await in awaits)
            {
                _startAwaits.Add(await);
            }
            return this;
        }
        /// <summary>
        /// Returns whether this event is executed and finished.
        /// </summary>
        public bool IsFinished() => HasBeenExecuted && AllFinishAwaitsFinished();
        /// <summary>
        /// Returns whether this event is allowed to start at this moment.
        /// </summary>
        public bool AllStartAwaitsFinished() => AllAwaitsFinished(_startAwaits);
        /// <summary>
        /// Returns whether this event is allowed to finish at this moment.
        /// </summary>
        public bool AllFinishAwaitsFinished() => AllAwaitsFinished(_finishAwaits);
        private bool AllAwaitsFinished(List<IAwaitable> awaits)
        {
            if (awaits == null) return true;

            foreach (var await in awaits)
            {
                if (!await.IsFinished())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
