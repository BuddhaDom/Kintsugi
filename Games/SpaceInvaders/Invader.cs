using Kintsugi.Core;
using Kintsugi.Physics;
using Shard;

namespace SpaceInvaders
{
    class Invader : GameObject, ICollisionHandler
    {

        // https://github.com/sausheong/invaders

        private int spriteToUse;
        private string[] sprites;
        private int xdir;
        private GameSpaceInvaders game;
        private Random rand;

        public int Xdir { get => xdir; set => xdir = value; }

        public override void Initialize()
        {
            sprites = new string[2];

            game = (GameSpaceInvaders)Bootstrap.GetRunningGame();

            sprites[0] = "invader1.png";
            sprites[1] = "invader2.png";

            spriteToUse = 0;

            this.Transform.X = 200.0f;
            this.Transform.Y = 100.0f;
            this.Transform.SpritePath = sprites[0];

            SetPhysicsEnabled();
            MyBody.AddRectCollider();

            rand = new Random();

            AddTag("Invader");

            MyBody.PassThrough = true;

        }


        public void changeSprite()
        {
            spriteToUse += 1;

            if (spriteToUse >= sprites.Length)
            {
                spriteToUse = 0;
            }

            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath(sprites[spriteToUse]);

        }

        public override void Update()
        {


            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public void OnCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.CheckTag("Player"))
            {
                x.Parent.ToBeDestroyed = true;
            }

            if (x.Parent.CheckTag("BunkerBit"))
            {
                x.Parent.ToBeDestroyed = true;
            }
        }

        public void OnCollisionExit(PhysicsBody x)
        {
        }

        public void OnCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Asteroid: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

        public void fire()
        {
            Bullet b = new Bullet();

            b.setupBullet(this.Transform.Centre.X, this.Transform.Centre.Y);
            b.Dir = 1;
            b.DestroyTag = "Player";
        }
    }
}
