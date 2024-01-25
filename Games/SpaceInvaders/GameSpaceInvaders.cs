using Kintsugi.Core;
using Kintsugi.Input;
using SpaceInvaders;
using System.Drawing;

namespace Shard
{
    class GameSpaceInvaders : Game, IInputListener
    {
        private Invader[,] myInvaders;
        private int xdir;
        private int moveCounter, moveDir;
        private float animCounter;
        private float timeToSwap;
        private int columns, rows;
        private int invaderCount;
        private bool dead;
        private List<Invader> livingInvaders;
        private Random rand;
        private GameObject ship;
        public int Xdir { get => xdir; set => xdir = value; }
        public bool Dead { get => dead; set => dead = value; }

        public override bool IsRunning()
        {
            if (ship == null || ship.ToBeDestroyed == true)
            {
                return false;
            }

            return true;

        }
        public override void Update()
        {
            Bootstrap.GetDisplay().ShowText("FPS: " + Bootstrap.GetFPS(), 10, 10, 12, 255, 255, 255);

            int ymod = 0;
            int deaths = 0;

            if (IsRunning() == false)
            {
                Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                Bootstrap.GetDisplay().ShowText("GAME OVER!", 300, 300, 128, col);
                return;
            }
            animCounter += (float)Bootstrap.GetDeltaTime();

            //            Debug.Log("Move Counter is " + moveCounter + ", dir is " + moveDir);

            if (animCounter > timeToSwap)
            {
                animCounter -= timeToSwap;

                livingInvaders.Clear();
                if (moveCounter > 12 || moveCounter <= -2)
                {
                    ymod = 50;
                    Xdir *= -1;
                    moveDir *= -1;

                }

                moveCounter += moveDir;


                invaderCount = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        if (myInvaders[i, j] == null || myInvaders[i, j].ToBeDestroyed == true)
                        {
                            deaths += 1;
                            continue;
                        }

                        // Speed them up as their numbers diminish.
                        timeToSwap = 2 - ((deaths / 3) * 0.1f);

                        myInvaders[i, j].changeSprite();

                        if (ymod != 0)
                        {
                            myInvaders[i, j].Transform.Translate(0, ymod);
                        }
                        else
                        {
                            myInvaders[i, j].Transform.Translate(xdir, 0);
                        }

                        livingInvaders.Add(myInvaders[i, j]);

                    }
                }

                Debug.Log("Living invaders" + livingInvaders.Count);

                // Pick a random invader to fire.
                livingInvaders[rand.Next(livingInvaders.Count)].fire();




            }

        }

        public void createObjects()
        {
            ship = new Spaceship();


            int ymod = 0;

            timeToSwap = 3;
            invaderCount = 0;
            livingInvaders = new List<Invader>();
            for (int j = 0; j < rows; j++)
            {
                for (int i = 0; i < columns; i++)
                {
                    Invader invader = new Invader();
                    invader.Transform.X = 100 + (i * 50);
                    invader.Transform.Y = 100 + (ymod * 50);

                    myInvaders[j, i] = invader;

                    livingInvaders.Add(myInvaders[j, i]);

                    invaderCount += 1;
                }

                ymod += 1;
            }

            moveDir = 1;
            Xdir = 35;

            // Finally, four bunkers

            for (int i = 0; i < 4; i++)
            {

                Bunker b = new Bunker();

                b.Transform.X = 200 + (i * 180);
                b.Transform.Y = 600;

                Debug.Log("Setting up bunker " + i + "at " + b.Transform.X + ", " + b.Transform.Y);

                b.setupBunker();

            }

            rand = new Random();

        }

        public override void Initialize()
        {
            Bootstrap.GetInput().AddListener(this);

            rows = 6;
            columns = 11;

            myInvaders = new Invader[rows, columns];
            createObjects();

            Debug.Log("Bing!");


        }

        public void HandleInput(InputEvent inp, string eventType)
        {


        }
    }
}
