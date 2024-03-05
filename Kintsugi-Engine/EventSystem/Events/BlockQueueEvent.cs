using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem.Events
{
    public class BlockQueueEvent : Event
    {
        public override bool BlockQueue() => true;
        public override void OnExecute()
        {
        }
    }
}
