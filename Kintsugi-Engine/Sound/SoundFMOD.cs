/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using SDL2;
using FMOD;

namespace Kintsugi.Audio
{
    // Please see https://www.fmod.com/docs/2.02/api/studio-guide.html#what-is-the-studio-api
    public class SoundFMOD : Sound
    {
        FMOD.System fmodCoreSystem;

        FMOD.Studio.System fmodSystem;
        // Destructors are not to be relied upong for termination, so might not get cleaned up...
        ~SoundFMOD()
        {
            fmodSystem.release();
            fmodCoreSystem.release();
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

        public override void Initialize()
        {
            // Docs says we have to call a core function to ensure loading so i chose to call this one, hopefully its not slow.
            FMOD.Memory.GetStats(out _, out _);


            var result = FMOD.Studio.System.create(out fmodSystem);
            if (result != FMOD.RESULT.OK)
            {
                throw new Exception("Fmod failed to load! + " + FMOD.Error.String(result));
            }

            // advanced settings here if relevant
            //fmodSystem.setAdvancedSettings();

            result = fmodSystem.initialize(512, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, 0);
            if (result != FMOD.RESULT.OK)
            {
                throw new Exception("Fmod failed to initialize! + " + FMOD.Error.String(result));
            }

            fmodSystem.getCoreSystem(out fmodCoreSystem);

            // consider if calling this is needed.
            //fmodCoreSystem.setSoftwareFormat();


        }

        public override void Update()
        {
            fmodSystem.update();
        }
    }
}

