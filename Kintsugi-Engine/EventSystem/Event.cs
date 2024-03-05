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
        private List<IAwaitable> _awaits;
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


        public Event AddAwait()
        {
            _awaits ??= new List<IAwaitable>();
            return this;
        }
        public Event AddAwaits(params IAwaitable[] awaits)
        {
            _awaits ??= new List<IAwaitable>();
            foreach (var await in awaits)
            {
                _awaits.Add(await);
            }
            return this;
        }

        public virtual bool IsFinished() => HasBeenExecuted;
        public bool AllAwaitsFinished()
        {
            if (_awaits == null) return true;

            foreach (var await in _awaits)
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
