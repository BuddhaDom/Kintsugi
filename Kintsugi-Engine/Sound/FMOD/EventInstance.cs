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
     * An instance of a single isntance of an event in memory that can be controlled and played.
     * </summary>
     */
    public class EventInstance
    {
        FMOD.Studio.EventInstance eventInstance;

        internal EventInstance(FMOD.Studio.EventInstance eventInstance)
        {
            this.eventInstance = eventInstance;
        }

        /**
         * <summary>
         * Plays and sets the event instance back to start position.
         * </summary>
         */
        public void Start()
        {
            SoundFMOD.ErrorCheck(eventInstance.start());
        }

        /**
         * <summary>
         * Mark for release. Will be released as soon as its done playing.
         * This will happen automatically when garbage collected, but if played often,
         * make sure to call release as soon as you are done calling functions on this instance.
         * </summary>
         */
        public void Release()
        {
            SoundFMOD.ErrorCheck(eventInstance.release());
        }

        /**
         * <summary>
         * Set local parameter of this instance.
         * </summary>
         */
        public void SetParameterByName(string parameterName, float value, bool ignoreSeekSpeed = false)
        {
            SoundFMOD.ErrorCheck(eventInstance.setParameterByName(parameterName, value, ignoreSeekSpeed));
        }
        /**
         * <summary>
         * Set local label parameter of this instance.
         * </summary>
         */
        public void SetParameterByNameWithLabel(string parameterName, string label, bool ignoreSeekSpeed = false)
        {
            SoundFMOD.ErrorCheck(eventInstance.setParameterByNameWithLabel(parameterName, label, ignoreSeekSpeed));
        }

        // Safety if developer never releases event. We prefer if the user releases, as this could happen late.
        ~EventInstance()
        {
            Console.WriteLine("Released event instance!");
            SoundFMOD.ErrorCheck(eventInstance.release());
        }
    }
}

