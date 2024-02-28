using Kintsugi.Core;
using PuzzleGame.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    internal class LevelManager
    {
        private static LevelManager _instance;
        public static LevelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelManager();
                }
                return _instance;
            }
        }


        public List<Level> Levels = new List<Level>{
            new Level1(),
            new Level2()
        };
        int currentIndex = -1;
        private Level curLevel;
        public void LoadNext()
        {
            if (curLevel != null)
            {
                curLevel.Unload();
            }

            currentIndex++;

            curLevel = Levels[currentIndex];
            curLevel.Load(Bootstrap.GetRunningGame());
        }
    }
}
