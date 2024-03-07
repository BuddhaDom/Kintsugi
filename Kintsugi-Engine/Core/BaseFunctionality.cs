/*
*
*   Generally useful functions that might be useful everywhere
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Core
{
    /// <summary>
    /// Core functions of the Kintsugi game engine.
    /// </summary>
    public class BaseFunctionality
    {
        private static BaseFunctionality me;

        private BaseFunctionality()
        {
        }

        /// <summary>
        /// Grab the instance of the BaseFunctionality class if it exists.
        /// </summary>
        /// <returns>New or existing instance of <see cref="BaseFunctionality"/></returns>
        public static BaseFunctionality GetInstance()
        {
            if (me == null)
            {
                me = new BaseFunctionality();
            }

            return me;
        }

        /// <summary>
        /// Read the contents of a file as text.
        /// </summary>
        /// <param name="file">Path to the file.</param>
        /// <returns>Text representation of the <paramref name="file"/>.</returns>
        public static string ReadFileAsString(string file)
        {
            string text;

            text = File.ReadAllText(file);

            return text;
        }

        /// <summary>
        /// Reads, processes and stores the specified config file.
        /// </summary>
        /// <param name="file">Path to the config file.</param>
        /// <returns>A dictionary of the config attribute and its corresponding configuration from the <paramref name="file"/></returns>
        public static Dictionary<string, string> ReadConfigFile(string file)
        {
            Dictionary<string, string> configEntries = new Dictionary<string, string>();
            string text = ReadFileAsString(file);
            string[] lines = text.Split("\n");
            string[] bits;
            string key, value;

            foreach (string line in lines)
            {
                // Don't read blank lines.
                if (line.Length == 0)
                {
                    continue;
                }

                // Don't read comments.
                if (line[0] == '#')
                {
                    continue;
                }

                bits = line.Split(":");

                key = bits[0].Trim();
                value = bits[1].Trim();

                value = value.Replace("%BASE_DIR%", Bootstrap.GetBaseDir());

                configEntries[key] = value;

                Console.WriteLine("Reading " + key + " and " + value);
            }

            return configEntries;
        }
    }
}
