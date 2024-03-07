using Kintsugi.Objects;

namespace Kintsugi.EventSystem.Events
{    
    /// <summary>
    /// An event that ends the turn for the given control group.
    /// </summary>
    internal class EndTurnEvent : Event
    {
        private ControlGroup controlGroup; 
        public EndTurnEvent(ControlGroup controlGroup)
        {
            this.controlGroup = controlGroup;
        }
        public override void OnExecute()
        {
            controlGroup.EndTurn();
        }
    }
}
