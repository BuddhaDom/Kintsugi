using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Await;
using Kintsugi.EventSystem.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EventSystem
{
    public class EventManager
    {
        internal List<Event> EventQueue = new();
        private static EventManager _instance = new();
        public static EventManager I
        {
            get => _instance;
        }

        public void Queue(Event @event)
        {
            EventQueue.Insert(EventQueue.Count, @event);
        }
        public void Queue(Action action)
        {
            EventQueue.Insert(EventQueue.Count, new ActionEvent(action));
        }

        public void QueueImmediate(Event @event)
        {
            EventQueue.Insert(0, @event);
        }
        public void QueueImmediate(Action action)
        {
            EventQueue.Insert(0, new ActionEvent(action));
        }

        internal void ProcessQueue()
        {
            for (int i = 0; i < EventQueue.Count; i++)
            {
                var currentEvent = EventQueue[i];
                // This event has already finished, keep going.
                if (currentEvent.IsFinished())
                {
                    EventQueue.RemoveAt(i);
                    i--;
                    continue;
                }
                // This event blocks the queue, so stop processing.
                if (currentEvent.BlockQueue())
                {
                    return;
                }
                // This event is still waiting for its dependencies
                if (!currentEvent.AllAwaitsFinished())
                {
                    continue;
                }

                // if it hasnt been executed yet, execute it.
                if (!currentEvent.HasBeenExecuted)
                {
                    currentEvent.Execute();
                }
            }
        }
    }
}
