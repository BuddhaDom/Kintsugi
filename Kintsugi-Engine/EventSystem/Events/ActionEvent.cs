namespace Kintsugi.EventSystem.Events
{
    /// <summary>
    /// An event that triggers the given action when executed.
    /// </summary>
    public class ActionEvent : Event
    {
        private Action action;
        public ActionEvent(Action action)
        {
            this.action = action;
        }
        public override void OnExecute()
        {
            action.Invoke();
        }
    }
}
