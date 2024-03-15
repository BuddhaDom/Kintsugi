using Kintsugi.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsGameTest.Events
{
    internal class DummyEvent : Kintsugi.EventSystem.Event
    {
        public override void OnExecute()
        {
        }
    }
}
