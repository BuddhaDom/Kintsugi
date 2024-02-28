using Kintsugi.EventSystem;
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
        public void QueueImmediate(Event @event)
        {
            EventQueue.Insert(0, @event);
        }

        internal void ProcessQueue()
        {
            while (EventQueue.Count > 0)
            {
                var currentEvent = EventQueue.First();
                EventQueue.RemoveAt(0);

                currentEvent.Execute();
            }
        }
    }
}
