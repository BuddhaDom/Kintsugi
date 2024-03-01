/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using SDL2;
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

            throw new NotImplementedException();
        }
        /**
         * <summary>
         * Load an FMOD bank and returns a wrapper.
         * </summary>
         */
        public Bank LoadBank(string path)
        {
            ErrorCheck(fmodSystem.loadBankFile(path, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out var bank));

            return new Bank(bank, this);
        }

        /**
         * <summary>
         * Load an FMOD event description and returns a wrapper.
         * </summary>
         */
        public EventDescription LoadEventDescription(string eventPath)
        {
            ErrorCheck(fmodSystem.getEvent(eventPath, out var _event));
            return new EventDescription(_event);
        }

        internal override void Initialize()
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

        /**
         * <summary>
         * Set local parameter of this instance.
         * </summary>
         */
        public void SetGlobalParameterByName(string parameterName, float value, bool ignoreSeekSpeed = false)
        {
            ErrorCheck(fmodSystem.setParameterByName(parameterName, value, ignoreSeekSpeed));
        }

        /**
         * <summary>
         * Set local label parameter of this instance.
         * </summary>
         */
        public void SetGlobalParameterByNameWithLabel(string parameterName, string label, bool ignoreSeekSpeed = false)
        {
            ErrorCheck(fmodSystem.setParameterByNameWithLabel(parameterName, label, ignoreSeekSpeed));
        }



        internal override void Update()
        {
            ErrorCheck(fmodSystem.update());
        }

        internal static void ErrorCheck(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                throw new FMODException(result);
            }
        }
    }
}

