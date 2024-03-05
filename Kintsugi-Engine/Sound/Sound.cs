/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Audio
{
    abstract public class Sound
    {
        abstract public void PlaySound(string file);
        abstract internal void Initialize();
        abstract internal void Update();
    }
}
