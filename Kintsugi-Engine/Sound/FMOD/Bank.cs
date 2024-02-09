/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Audio
{
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
}

