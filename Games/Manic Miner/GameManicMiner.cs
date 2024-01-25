// Sprites from https://www.spriters-resource.com/fullview/113060/

using Kintsugi.Core;
using Kintsugi.Input;
using ManicMiner;
using System.Drawing;

namespace Shard
{
    class GameManicMiner : Game, IInputListener
    {
        Random rand;
        MinerWilly player;
        List<Collectible> collect;

        public override bool IsRunning()
        {

            if (player == null || player.ToBeDestroyed)
            {
                return false;
            }

            foreach (Collectible c in collect)
            {
                if (c != null && !c.ToBeDestroyed)
                {
                    return true;
                }
            }

            return false;

        }

        public override void Update()
        {

            if (IsRunning() == false)
            {
                Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                Bootstrap.GetDisplay().ShowText("GAME OVER!", 300, 300, 128, col);
                return;
            }


        }

        public override void Initialize()
        {
            Platform p;
            Bootstrap.GetInput().AddListener(this);
            rand = new Random();

            player = new MinerWilly();
            collect = new List<Collectible>();

            p = new Platform();
            p.SetPosition(0, 750, 256, 50);

            for (int i = 0; i < 10; i++)
            {
                p = new Platform();

                if (i == 2)
                {
                    p.SetPosition(450, 800, 256, 50);
                    p.MoveDirX = 1;
                }
                else
                {
                    p.SetPosition(0 + (i * 256), 832, 0, 0);
                }

            }


            p = new Platform();
            p.SetPosition(30, 800, 600, 150);

            p = new Platform();
            p.SetPosition(128, 780, 600, 150);
            p.MoveDirY = -1;

            p = new Platform();
            p.SetPosition(400, 605, 600, 100);
            p.MoveDirY = -1;

            p = new Platform();
            p.SetPosition(600, 700, 700, 200);
            p.MoveDirY = -1;

            p = new Platform();
            p.SetPosition(800, 105, 600, 200);

            p = new Platform();
            p.SetPosition(800, 605, 600, 200);



            Collectible c = new Collectible();
            c.Transform.Translate(100, 780);
            collect.Add(c);


            c = new Collectible();
            c.Transform.Translate(840, 560);
            collect.Add(c);

            c = new Collectible();
            c.Transform.Translate(840, 60);
            collect.Add(c);

        }


        public void HandleInput(InputEvent inp, string eventType)
        {
        }
    }
}
