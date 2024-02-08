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
using FMOD.Studio;
using System.IO;

namespace Kintsugi.Audio
{
    // Please see https://www.fmod.com/docs/2.02/api/studio-guide.html#what-is-the-studio-api
    public class SoundFMOD : Sound
    {
        internal FMOD.System fmodCoreSystem;

        internal FMOD.Studio.System fmodSystem;
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

        public Bank LoadBank(string path)
        {
            ErrorCheck(fmodSystem.loadBankFile(path, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out var bank));

            return new Bank(bank, this);
        }
        public Event LoadEvent(string eventPath)
        {
            ErrorCheck(fmodSystem.getEvent(eventPath, out var _event));
            return new Event(_event);
        }
        public override void Initialize()
        {

            Console.WriteLine("Initializing fmod!");
            // NOTE: THIS IS A LOAD BEARING FUNCTION CALL DO NOT REMOVE.
            // we need to call something from the FMOD dll before FMOD studio dll to ensure its loaded first.
            Console.WriteLine("FMOD version: {0:X}", FMOD.VERSION.number); // the version number is stored in hexadecimal for some reason.



            ErrorCheck(FMOD.Studio.System.create(out fmodSystem));

            // advanced settings here if relevant
            //fmodSystem.setAdvancedSettings();

            ErrorCheck(fmodSystem.initialize(512, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, 0));
            ErrorCheck(fmodSystem.getCoreSystem(out fmodCoreSystem));

            // consider if calling this is needed.
            //fmodCoreSystem.setSoftwareFormat();


        }

        public override void Update()
        {
            ErrorCheck(fmodSystem.update());
        }

        internal static void ErrorCheck(RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                throw new Exception("Fmod error! + " + FMOD.Error.String(result));
            }
        }
    }


    public class Bank
    {
        SoundFMOD fmod;
        FMOD.Studio.Bank bank;
        internal Bank(FMOD.Studio.Bank bank, SoundFMOD fmod)
        {
            this.fmod = fmod;
            this.bank = bank;
        }

        public void PreloadSamples()
        {
            SoundFMOD.ErrorCheck(bank.loadSampleData());
        }
        public void UnloadSamples()
        {
            SoundFMOD.ErrorCheck(bank.unloadSampleData());
        }
    }
    public class Event
    {
        SoundFMOD soundFMOD;
        FMOD.Studio.EventDescription eventDescription;
        internal Event(FMOD.Studio.EventDescription eventDescription)
        {
            this.eventDescription = eventDescription;
        }

        /**
         * <summary>Creates an instance of the event, plays it immediately, and releases it.</summary>
         */
        public void PlayImmediate()
        {
            SoundFMOD.ErrorCheck(eventDescription.createInstance(out var instance));
            SoundFMOD.ErrorCheck(instance.start());
            SoundFMOD.ErrorCheck(instance.release());
        }
    }
}

