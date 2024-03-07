/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Audio
{
    /**
     * <summary>
     * The description of an event in FMOD. Can be used to preload and play instances of the event.
     * </summary>
     */
    public class EventDescription
    {
        SoundFMOD soundFMOD;
        FMOD.Studio.EventDescription eventDescription;
        internal EventDescription(FMOD.Studio.EventDescription eventDescription)
        {
            this.eventDescription = eventDescription;
        }

        /**
         * <summary>Loads all sample data for the event into memory, 
         * so they need not be loaded on play, and play instantly.
         * Not needed if the bank itself is already loaded.</summary>
         */
        public void PreloadSamples()
        {
            SoundFMOD.ErrorCheck(eventDescription.loadSampleData());
        }

        /**
         * <summary>Unloads all sample data for events from memory.
         * Events will still automatically load and unload on play.</summary>
         */
        public void UnloadSamples()
        {
            SoundFMOD.ErrorCheck(eventDescription.unloadSampleData());
        }


        /**
         * <summary>Creates an instance of the event, plays it immediately, and releases it.
         * Useful for simple oneshots, especially if they are played simultaneausly.</summary>
         */
        public void PlayImmediate()
        {
            SoundFMOD.ErrorCheck(eventDescription.createInstance(out var instance));
            SoundFMOD.ErrorCheck(instance.start());
            SoundFMOD.ErrorCheck(instance.release());
            //eventDescription.setCallback(Callback);
        }

        /**
         * <summary>Returns an instance of the event.</summary>
         */
        public EventInstance CreateInstance()
        {
            SoundFMOD.ErrorCheck(eventDescription.createInstance(out var instance));
            return new EventInstance(instance);
        }

        // This is if we want to implement callbacks. they seem cool.
        /*
        public RESULT Callback(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
        {

            Console.WriteLine("Callback: " + type);
            return RESULT.OK;
        }
        */
    }
}

