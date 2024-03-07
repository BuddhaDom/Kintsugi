namespace Kintsugi.EventSystem.Events
{    /// <summary>
     /// An event that blocks the queue until finished.
     /// </summary>
    public class BlockQueueEvent : Event
    {
        public override bool BlockQueue() => true;
        public override void OnExecute()
        {
        }
    }
}
