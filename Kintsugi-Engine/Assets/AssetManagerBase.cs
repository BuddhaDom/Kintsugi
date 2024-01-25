namespace Kintsugi.Assets
{
    public abstract class AssetManagerBase
    {
        private string assetPath;

        public string AssetPath { get; set; }

        public abstract void RegisterAssets();
        public abstract string GetAssetPath(string asset);
    }

}
