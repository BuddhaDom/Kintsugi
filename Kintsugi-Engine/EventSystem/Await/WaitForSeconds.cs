using Kintsugi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem.Await
{
    /// <summary>
    /// Awaitable that will return finished a given amount of seconds after instantiated.
    /// </summary>
    internal class WaitForSeconds: IAwaitable
    {
        private double endTime = double.PositiveInfinity;
        public WaitForSeconds(float seconds)
        {
            endTime = Bootstrap.TimeElapsed + seconds;
        }
        public bool IsFinished() => Bootstrap.TimeElapsed >= endTime;
    }
}
