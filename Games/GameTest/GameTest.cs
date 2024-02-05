using GameTest;
using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Tiles;

namespace Shard
{
    class GameTest : Game, IInputListener
    {
        GameObject background;
        List<GameObject> asteroids;
        private GameObject grid;
        public override void Update()
        {

            Bootstrap.GetDisplay().ShowText("FPS: " + Bootstrap.GetSecondFPS() + " / " + Bootstrap.GetFPS(), 10, 10, 12, 255, 255, 255);


        }

        public override int GetTargetFrameRate()
        {
            return 100;

        }
        public void CreateShip()
        {
            GameObject ship = new Spaceship();
            Random rand = new Random();
            int offsetx = 0, offsety = 0;

            GameObject asteroid;




            //            asteroid.MyBody.Kinematic = true;



            background = new GameObject();
            background.Transform.SpritePath = GetAssetManager().GetAssetPath("background2.jpg");
            background.Transform.X = 0;
            background.Transform.Y = 0;


        }

        public override void Initialize()
        {
            Bootstrap.GetInput().AddListener(this);
            CreateShip();

            asteroids = new List<GameObject>();

            grid = new Grid(GetAssetManager().GetAssetPath("forestpath.tmx"));
            grid.Transform.X = 70;
            grid.Transform.Y = 70;
        }

        public void HandleInput(InputEvent inp, string eventType)
        {

            if (eventType == "MouseDown")
            {
                Console.WriteLine("Pressing button " + inp.Button);
            }

            if (eventType == "MouseDown" && inp.Button == 1)
            {
                Asteroid asteroid = new Asteroid();
                asteroid.Transform.X = inp.X;
                asteroid.Transform.Y = inp.Y;
                asteroids.Add(asteroid);
            }

            if (eventType == "MouseDown" && inp.Button == 3)
            {
                foreach (GameObject ast in asteroids)
                {
                    ast.ToBeDestroyed = true;
                }

                asteroids.Clear();
            }


        }
    }
}
