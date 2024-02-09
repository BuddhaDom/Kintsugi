/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Audio
{
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

