using Kintsugi.Core;
using Kintsugi.EventSystem.Await;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem.Events
{
    /// <summary>
    /// An event that blocks the queue for the given amount of seconds after executed.
    /// </summary>
    public class BlockQueueForSeconds : Event
    {
        private float seconds;
        public BlockQueueForSeconds(float seconds)
        {
            shouldBlockQueue = true;
            this.seconds = seconds;
        }

        public override void OnExecute()
        {
            AddFinishAwait(new WaitForSeconds(seconds));
        }
    }
}
