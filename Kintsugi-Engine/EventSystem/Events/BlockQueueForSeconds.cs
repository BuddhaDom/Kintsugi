using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem.Events
{
    public class BlockQueueForSeconds : Event
    {
        private double endTime;
        public BlockQueueForSeconds(float seconds)
        {
            endTime = Bootstrap.TimeElapsed + seconds;
        }
        public override bool BlockQueue() => true;
        public override bool IsFinished() => Bootstrap.TimeElapsed > endTime;


        public override void OnExecute()
        {
        }
    }
}
