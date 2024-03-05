using Kintsugi.EventSystem.Await;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem
{
    public abstract class Event: IAwaitable
    {
        private List<IAwaitable> _startAwaits;
        private List<IAwaitable> _finishAwaits;
        public bool HasBeenExecuted { get; protected set; }
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
        public abstract void OnExecute();
        protected bool shouldBlockQueue;
        public virtual bool BlockQueue () { return shouldBlockQueue; }


        public Event SetAsQueueBlocker()
        {
            shouldBlockQueue = true;
            return this;
        }


        public Event AddFinishAwait(IAwaitable await)
        {
            _finishAwaits ??= new List<IAwaitable>();
            _finishAwaits.Add(await);
            return this;
        }
        public Event AddFinishAwait(params IAwaitable[] awaits)
        {
            _finishAwaits ??= new List<IAwaitable>();
            foreach (var await in awaits)
            {
                _finishAwaits.Add(await);
            }
            return this;
        }


        public Event AddStartAwait(IAwaitable await)
        {
            _startAwaits ??= new List<IAwaitable>();
            _startAwaits.Add(await);
            return this;
        }
        public Event AddStartAwaits(params IAwaitable[] awaits)
        {
            _startAwaits ??= new List<IAwaitable>();
            foreach (var await in awaits)
            {
                _startAwaits.Add(await);
            }
            return this;
        }

        public virtual bool IsFinished() => HasBeenExecuted && AllFinishAwaitsFinished();
        public bool AllStartAwaitsFinished() => AllAwaitsFinished(_startAwaits);
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
