namespace Kintsugi.Assets
{
    /// <summary>
    /// Base form of the AssetManager class. Can be used for implementing different forms of asset loading.
    /// </summary>
    public abstract class AssetManagerBase
    {
        private string assetPath;
        /// <summary>
        /// Absolute path to the asset directory.
        /// </summary>
        public string AssetPath { get; set; }
        /// <summary>
        /// Register all assets in asset directory.
        /// Must be done before assets can be loaded.
        /// </summary>
        public abstract void RegisterAssets();
        /// <summary>
        /// Get absolute path of registered asset with asset path <paramref name="asset"/>
        /// </summary>
        public abstract string GetAssetPath(string asset);
    }

}
