using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem.Events
{
    internal class ActionEvent : Event
    {
        private Action action;
        public ActionEvent(Action action)
        {
            this.action = action;
        }
        public override void Execute()
        {
            action.Invoke();
        }
    }
}
