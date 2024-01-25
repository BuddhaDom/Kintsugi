/*
*
*   Some handy debug methods.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Core
{
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

        public static Debug GetInstance()
        {
            if (me == null)
            {
                me = new Debug();
            }

            return me;
        }

        public void SetDebugLevel(int d)
        {
            debugLevel = d;
        }

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

        public void LogInst(string message)
        {
            Log(message, DEBUG_LEVEL_ALL);
        }


        public static void Log(string message)
        {
            GetInstance().Log(message, DEBUG_LEVEL_ALL);
        }

    }
}
