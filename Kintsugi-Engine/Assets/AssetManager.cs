using Kintsugi.Core;

namespace Kintsugi.Assets
{
    public class AssetManager : AssetManagerBase
    {

        private Dictionary<string, string> assets;

        public AssetManager()
        {
            assets = new Dictionary<string, string>();
            AssetPath = Bootstrap.GetEnvironmentalVariable("assetpath");
        }

        public override void RegisterAssets()
        {
            assets.Clear();
            WalkDirectory("");
        }

        internal string GetName(string path)
        {
            string[] bits = path.Split("\\");

            return bits[bits.Length - 1];
        }

        public override string GetAssetPath(string asset)
        {
            if (assets.TryGetValue(asset, out string? value))
            {
                return value;
            }

            Debug.Log("No entry for " + asset);

            return null;
        }

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
