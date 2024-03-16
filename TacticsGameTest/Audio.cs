using Kintsugi.Audio;
using Kintsugi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void PlayAudio(String path)
        {
            var audio = ((SoundFMOD)Bootstrap.GetSound());
            audio.LoadEventDescription("event:/" + path).PlayImmediate();
        }

    }
}
