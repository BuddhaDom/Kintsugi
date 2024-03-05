using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem.Events
{
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
