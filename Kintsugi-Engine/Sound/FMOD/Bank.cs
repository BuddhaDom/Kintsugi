namespace Kintsugi.Audio
{
    /**
     * <summary>
     * The description of a bank in FMOD. Must be loaded before any events in the bank can be played.
     * </summary>
     */
    public class Bank
    {
        SoundFMOD fmod;
        FMOD.Studio.Bank bank;
        internal Bank(FMOD.Studio.Bank bank, SoundFMOD fmod)
        {
            this.fmod = fmod;
            this.bank = bank;
        }

        /**
         * <summary>Loads all sample data for associated events into memory, 
         * so they need not be loaded on play, and play instantly.</summary>
         */
        public void PreloadSamples()
        {
            SoundFMOD.ErrorCheck(bank.loadSampleData());
        }

        /**
         * <summary>Unloads all sample data for associated events from memory.
         * Events will still automatically load and unload on play.</summary>
         */
        public void UnloadSamples()
        {
            SoundFMOD.ErrorCheck(bank.unloadSampleData());
        }


        /**
         * <summary>Unloads entire bank from memory, including metadata.
         * Events from this bank will no longer be able to be played.</summary>
         */
        public void Unload()
        {
            bank.unload();
        }
    }
}

