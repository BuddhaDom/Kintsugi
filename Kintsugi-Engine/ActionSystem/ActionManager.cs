using Kintsugi.ActionSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.ActionSystem
{
    public class ActionManager
    {
        internal List<BaseAction> ActionQueue = new();
        private static ActionManager _instance = new();

        public ActionManager I
        {
            get => _instance;
        }

        public void Queue(BaseAction action)
        {
            ActionQueue.Insert(ActionQueue.Count, action);
        }
        public void QueueImmediate(BaseAction action)
        {
            ActionQueue.Insert(0, action);
        }

        internal void ProcessQueue()
        {
            while (ActionQueue.Count > 0)
            {
                var currentAction = ActionQueue.First();
                ActionQueue.RemoveAt(0);

                currentAction.Execute();
            }
        }
    }
}
