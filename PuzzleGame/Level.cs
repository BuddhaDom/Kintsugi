using Kintsugi.Core;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    internal abstract class Level
    {
        public Grid grid;
        public abstract void Load(Game game);
        public void Unload()
        {
            grid = null;
        }
    }
}
