using Kintsugi.Audio;
using Kintsugi.Core;

namespace TacticsGameTest
{
  
    internal class Audio
    {
        private static Audio _instance;
        public static Audio I
        {
            get { if (_instance == null)
                    _instance = new Audio();
            return _instance;}
        }
        public EventInstance bgfx = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/DungeonAmbience").CreateInstance();
        public EventInstance music = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/Music").CreateInstance();
        public EventInstance musicExplore = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/MusicMap").CreateInstance();
        public EventInstance musicBoss = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/MusicBoss").CreateInstance();

        public void PlayAudio(String path)
        {
            var audio = ((SoundFMOD)Bootstrap.GetSound());
            audio.LoadEventDescription("event:/" + path).PlayImmediate();
        }

    }
}
