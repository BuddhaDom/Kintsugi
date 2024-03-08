using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Events;

namespace Engine.EventSystem
{
    /// <summary>
    /// Manager class for events in the engine.
    /// </summary>
    public class EventManager
    {
        internal List<Event> EventQueue = new();
        private static EventManager _instance = new();
        public static EventManager I
        {
            get => _instance;
        }
        /// <summary>
        /// Removes all events in the queue.
        /// </summary>
        public void ClearQueue()
        {
            EventQueue.Clear();
        }
        /// <summary>
        /// Add event to the start of the queue.
        /// </summary>
        public void Queue(Event @event)
        {
            EventQueue.Insert(EventQueue.Count, @event);
        }
        /// <summary>
        /// Add action as an event to the start of the queue.
        /// </summary>
        public void Queue(Action action)
        {
            EventQueue.Insert(EventQueue.Count, new ActionEvent(action));
        }
        /// <summary>
        /// Add event to the end of the queue.
        /// </summary>
        public void QueueImmediate(Event @event)
        {
            EventQueue.Insert(0, @event);
        }
        /// <summary>
        /// Add action as an event to the end of the queue.
        /// </summary>
        public void QueueImmediate(Action action)
        {
            EventQueue.Insert(0, new ActionEvent(action));
        }
        /// <summary>
        /// Checks if any event is being processed in the queue.
        /// </summary>
        public bool IsQueueDone()
        {
            return EventQueue.Count == 0;
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
                // This event is still waiting for its dependencies
                if (!currentEvent.AllStartAwaitsFinished())
                {
                    if (currentEvent.BlockQueue())
                    {
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
                // if it hasnt been executed yet, execute it.
                if (!currentEvent.HasBeenExecuted)
                {
                    currentEvent.Execute();
                }
                // This event blocks the queue, so stop processing.
                if (currentEvent.BlockQueue())
                {
                    return;
                }

            }
        }
    }
}
