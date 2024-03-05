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
        /// <summary>
        /// Make an <see cref="IAwaitable"/> wait for an amount of seconds.
        /// </summary>
        /// <param name="seconds">How long to wait.</param>
        public WaitForSeconds(float seconds)
        {
            endTime = Bootstrap.TimeElapsed + seconds;
        }
        /// <summary>
        /// Check if the waiting has finished.
        /// </summary>
        /// <returns><c>true</c> if the awaitable has finished.</returns>
        public bool IsFinished() => Bootstrap.TimeElapsed >= endTime;
    }
}
