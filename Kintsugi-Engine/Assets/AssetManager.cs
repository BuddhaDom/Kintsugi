using Kintsugi.Core;

namespace Kintsugi.Assets
{
    /// <summary>
    /// Manager class for asset acquisition and loading.
    /// </summary>
    public class AssetManager : AssetManagerBase
    {

        private Dictionary<string, string> assets;

        public AssetManager()
        {
            assets = new Dictionary<string, string>();
            AssetPath = Bootstrap.GetEnvironmentalVariable("assetpath");
        }

        /// <summary>
        /// Initialization function for the private asset collection.
        /// </summary>
        public override void RegisterAssets()
        {
            assets.Clear();
            WalkDirectory("");
        }

        /// <summary>
        /// Get the name and extension of a file.
        /// </summary>
        /// <param name="path">Absolute path to the file.</param>
        /// <returns>File name and extension.</returns>
        internal string GetName(string path)
        {
            string[] bits = path.Split("\\");

            return bits[bits.Length - 1];
        }

        /// <summary>
        /// Gets the absolute path of an asset based on its relative path in the assets folder.
        /// </summary>
        /// <param name="asset">An asset in the assets directory.</param>
        /// <returns>The absolute path of the file.</returns>
        public override string GetAssetPath(string asset)
        {
            if (assets.TryGetValue(asset, out string? value))
            {
                return value;
            }

            Debug.Log("No entry for " + asset);

            return null;
        }
        
        /// <summary>
        /// Recurse over directories to store assets into the private collection of directories. 
        /// </summary>
        /// <param name="relativeDir">Relative directory of this recursion.</param>
        internal void WalkDirectory(string relativeDir)
        {
            string absoluteDir = AssetPath + relativeDir;
            string[] files = Directory.GetFiles(absoluteDir);
            string[] dirs = Directory.GetDirectories(absoluteDir);

            foreach (string d in dirs)
            {
                WalkDirectory(relativeDir + GetName(d) + "\\");
            }

            foreach (string f in files)
            {
                string filename_raw = GetName(f);
                string assetPath = relativeDir + filename_raw;
                int counter = 0;

                Console.WriteLine("Loading asset " + assetPath);

                while (assets.ContainsKey(assetPath))
                {
                    counter += 1;
                    assetPath = filename_raw + counter;
                }

                assets.Add(assetPath, f);
                Console.WriteLine("Adding " + assetPath + " : " + f);
            }

        }



    }
}
