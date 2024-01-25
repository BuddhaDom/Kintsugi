/*
*
*   The Game class is the entry way to the system, and it's set in the config file.  The overarching class
*       that drives your game (think of it as your main program) should extend from this.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Assets;

namespace Kintsugi.Core
{
    public abstract class Game
    {
        public void Run()
        {
            Console.WriteLine("Starting game...");
            Bootstrap.RunStuff(this);
        }


        public AssetManagerBase assets;

        public AssetManagerBase GetAssetManager()
        {
            if (assets == null)
            {
                assets = Bootstrap.GetAssetManager();
            }

            return assets;
        }

        public abstract void Initialize();
        public abstract void Update();

        public virtual bool IsRunning()
        {
            return true;
        }

        // By default our games will run at the maximum speed possible, but 
        // note that we have millisecond timing precision.  Any frame rate that 
        // needs greater precision than that will start to go... weird.
        public virtual int GetTargetFrameRate()
        {
            return int.MaxValue;
        }

    }
}
