using Kintsugi.Core;
using Kintsugi.Input;
using MissileCommand;
using System.Drawing;

namespace Shard
{
    class GameMissileCommand : Game, IInputListener
    {
        List<Missile> myMissiles;
        List<Arsenal> myArsenals;
        List<City> cities;
        Random rand;
        double counter;
        double timeBetweenMissiles = 3;
        int missileChance = 1;

        public override int GetTargetFrameRate()
        {
            return 1000;
        }
        public override bool IsRunning()
        {
            foreach (City c in cities)
            {
                if (c != null && c.ToBeDestroyed == false)
                {
                    return true;
                }
            }

            return false;

        }

        public override void Update()
        {
            bool fired = false;

            if (IsRunning() == false)
            {
                Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
                Bootstrap.GetDisplay().ShowText("GAME OVER!", 300, 300, 128, col);
                return;
            }

            counter += Bootstrap.GetDeltaTime();

            if (counter > 0.5f)
            {
                // We do it this way to add a little uncertainty regaridng timing.
                fired = generateIncoming();

                if (fired)
                {
                    counter = 0;
                }
            }


        }

        public override void Initialize()
        {

            int imod = 0;
            Bootstrap.GetInput().AddListener(this);
            counter = 0;
            cities = new List<City>();

            for (int i = 0; i < 6; i++)
            {
                City c = new City();

                cities.Add(c);

                if (i == 3)
                {
                    imod = 200;
                }

                c.Transform.Translate(100 + imod + (i * 140), 750);

            }

            myArsenals = new List<Arsenal>();

            for (int i = 0; i < 3; i++)
            {
                Arsenal a = new Arsenal();
                a.Transform.Translate(25 + (i * 550), 700);
                a.resetMissiles();

                myArsenals.Add(a);
            }

            rand = new Random();

        }

        public bool generateIncoming()
        {
            List<City> theCities;
            Missile m;
            City target;

            if (rand.Next(0, 1000) > missileChance)
            {

                // We're not firing one.
                return false;
            }

            theCities = new List<City>();

            foreach (City c in cities)
            {
                if (c == null || c.ToBeDestroyed == true)
                {
                    continue;
                }

                theCities.Add(c);
            }

            // generate an incoming missile

            m = new Missile();
            m.Transform.Translate(rand.Next(0, Bootstrap.GetDisplay().GetWidth()), 0);

            m.Originx = (float)m.Transform.X;
            m.Originy = (float)m.Transform.Y;

            target = theCities[rand.Next(0, theCities.Count)];

            m.Targetx = (float)target.Transform.Centre.X;
            m.Targety = (float)target.Transform.Centre.Y;

            // Some of our missiles will split before they explode.
            if (rand.Next(0, 100) < 10)
            {
                m.Mirv = true;
            }

            m.AddTag("EnemyMissile");
            m.TargetTag = "City";
            m.Speed = 10;
            m.MyColor = Color.Red;
            m.TheTargets = theCities;

            return true;
        }


        public void HandleInput(InputEvent inp, string eventType)
        {
            Arsenal a;
            int which = -1;
            if (eventType == "MouseDown")
            {

                if (inp.Button == 3)
                {
                    // Right mouse button.
                    which = 2;
                }

                if (inp.Button == 2)
                {
                    // Middle mouse button.
                    which = 1;
                }

                if (inp.Button == 1)
                {
                    // Middle mouse button.
                    which = 0;
                }

                if (which == -1)
                {
                    // Who knows?
                    which = rand.Next(0, 2);
                }

                Missile m = new Missile();

                Debug.Log("Pressed button " + inp.Button);
                a = myArsenals[which];

                if (a.canFireMissile() == false)
                {
                    return;
                }

                a.fireMissile();

                m.Originx = (float)a.Transform.Centre.X;
                m.Originy = (float)a.Transform.Centre.Y;

                m.Transform.Translate(m.Originx, m.Originy);

                m.Transform.X = m.Originx;
                m.Transform.Y = m.Originy;

                m.Targetx = inp.X;
                m.Targety = inp.Y;

                m.AddTag("PlayerMissile");
                m.TargetTag = "EnemyMissile";

                m.Speed = 1000;
                m.MyColor = Color.Green;



            }
        }
    }
}
