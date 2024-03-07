/*
*
*   Some handy debug methods.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Core
{
    /// <summary>
    /// Methods allowing debugging of the engine. Debug manager.
    /// </summary>
    public class Debug
    {
        public static readonly int DEBUG_LEVEL_NONE = 0;
        public static readonly int DEBUG_LEVEL_ERROR = 1;
        public static readonly int DEBUG_LEVEL_WARNING = 2;
        public static readonly int DEBUG_LEVEL_ALL = 3;

        private static Debug me;
        private int debugLevel;

        private Debug()
        {
            debugLevel = DEBUG_LEVEL_ALL;
        }

        /// <summary>
        /// Get the existing instance of the debug class or create one.
        /// </summary>
        /// <returns>The debug manager.</returns>
        public static Debug GetInstance()
        {
            if (me == null)
            {
                me = new Debug();
            }

            return me;
        }

        /// <summary>
        /// Set the debug level.
        /// </summary>
        /// <param name="d">Debug level from 0 to 3.</param>
        public void SetDebugLevel(int d)
        {
            debugLevel = d;
        }

        /// <summary>
        /// Output a message at a given level of debug.
        /// </summary>
        /// <param name="message">Message to output.</param>
        /// <param name="level">Debug level of this log.</param>
        public void Log(string message, int level)
        {
            if (debugLevel == DEBUG_LEVEL_NONE)
            {
                return;
            }

            if (level <= debugLevel)
            {
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Add something to the log at level <see cref="DEBUG_LEVEL_ALL"/>
        /// </summary>
        /// <param name="message">Message to output.</param>
        public void LogInst(string message)
        {
            Log(message, DEBUG_LEVEL_ALL);
        }
        
        /// <summary>
        /// Add something to the existing log instance at level <see cref="DEBUG_LEVEL_ALL"/>
        /// </summary>
        /// <param name="message">Message to output.</param>
        public static void Log(string message)
        {
            GetInstance().Log(message, DEBUG_LEVEL_ALL);
        }

    }
}
