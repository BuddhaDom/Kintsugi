using Kintsugi.Core;
using Kintsugi.Input;
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
            new Level2(),
            new Level3(),
            new Level4(),
            new Level5(),
            new Level6(),
            new Level7(),
            new cutscene_EasterEgg()
        };
        int currentIndex = -1;
        private Level curLevel;

        private List<IInputListener> levelInputListeners;
        public void AddLevelListener(IInputListener listener)
        {
            curLevel.AddLevelInputListener(listener);
        }
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
        public void ResetLevel()
        {
            if (curLevel != null)
            {
                curLevel.Unload();
                curLevel.Load(Bootstrap.GetRunningGame());
            }
        }

    }
}
