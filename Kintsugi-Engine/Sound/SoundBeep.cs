/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using SDL2;

namespace Kintsugi.Audio
{
    public class SoundSDL : Sound
    {
        internal override void Initialize()
        {
        }

        public override void PlaySound(string file)
        {

            SDL.SDL_AudioSpec have, want;
            uint length, dev;
            nint buffer;

            file = Bootstrap.GetAssetManager().GetAssetPath(file);

            SDL.SDL_LoadWAV(file, out have, out buffer, out length);
            dev = SDL.SDL_OpenAudioDevice(nint.Zero, 0, ref have, out want, 0);

            int success = SDL.SDL_QueueAudio(dev, buffer, length);
            SDL.SDL_PauseAudioDevice(dev, 0);

        }

        internal override void Update()
        {
        }
    }
}

