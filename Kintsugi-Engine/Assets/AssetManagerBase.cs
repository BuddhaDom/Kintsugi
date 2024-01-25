namespace Kintsugi.Assets
{
    public abstract class AssetManagerBase
    {
        private string assetPath;

        public string AssetPath { get; set; }

        public abstract void registerAssets();
        public abstract string getAssetPath(string asset);
    }

}
