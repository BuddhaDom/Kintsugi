using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
