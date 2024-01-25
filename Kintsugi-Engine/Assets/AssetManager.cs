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
            WalkDirectory(AssetPath);
        }

        public string GetName(string path)
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

        public void WalkDirectory(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            string[] dirs = Directory.GetDirectories(dir);

            foreach (string d in dirs)
            {
                WalkDirectory(d);
            }

            foreach (string f in files)
            {
                string filename_raw = GetName(f);
                string filename = filename_raw;
                int counter = 0;

                Console.WriteLine("Filename is " + filename);

                while (assets.ContainsKey(filename))
                {
                    counter += 1;
                    filename = filename_raw + counter;
                }

                assets.Add(filename, f);
                Console.WriteLine("Adding " + filename + " : " + f);
            }

        }



    }
}
