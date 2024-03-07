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
    /// <summary>
    /// The game the engine should run.
    /// </summary>
    public abstract class Game
    {
        /// <summary>
        /// Start the game's program.
        /// </summary>
        public void Run()
        {
            Console.WriteLine("Starting game...");
            Bootstrap.RunStuff(this);
        }

        /// <summary>
        /// Asset manager system.
        /// </summary>
        public AssetManagerBase assets;

        /// <summary>
        /// Fetch the asset manager.
        /// </summary>
        /// <returns>The asset manager system.</returns>
        public AssetManagerBase GetAssetManager()
        {
            if (assets == null)
            {
                assets = Bootstrap.GetAssetManager();
            }

            return assets;
        }

        /// <summary>
        /// Initialize the game. Called once.
        /// </summary>
        public abstract void Initialize();
        /// <summary>
        /// Update the game. Called every frame.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Checks if this game is currently running.
        /// </summary>
        /// <returns><c>true</c> if the game is running.</returns>
        public virtual bool IsRunning()
        {
            return true;
        }

        // By default our games will run at the maximum speed possible, but 
        // note that we have millisecond timing precision.  Any frame rate that 
        // needs greater precision than that will start to go... weird.
        /// <summary>
        /// Get the target framerate of the game.
        /// </summary>
        /// <returns>Target frames per second.</returns>
        public virtual int GetTargetFrameRate()
        {
            return int.MaxValue;
        }

    }
}
