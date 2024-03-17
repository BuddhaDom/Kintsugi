/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Audio
{
    /// <summary>
    /// Base class for sound engines.
    /// </summary>
    abstract public class Sound
    {
        /// <summary>
        /// Load and play a sound file.
        /// </summary>
        /// <param name="file">Path to the sound file.</param>
        abstract public void PlaySound(string file);
        /// <summary>
        /// Initialize the sound engine.
        /// </summary>
        abstract internal void Initialize();
        /// <summary>
        /// Update the sound engine.
        /// </summary>
        abstract internal void Update();
    }
}
