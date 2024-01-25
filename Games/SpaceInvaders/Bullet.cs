using Kintsugi.Core;
using Kintsugi.Physics;
using Shard;
using System.Drawing;

namespace SpaceInvaders
{
    class Bullet : GameObject, ICollisionHandler
    {
        private string destroyTag;
        private int dir;

        public string DestroyTag { get => destroyTag; set => destroyTag = value; }
        public int Dir { get => dir; set => dir = value; }

        public void setupBullet(float x, float y)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = 1;
            this.Transform.Ht = 20;


            SetPhysicsEnabled();

            MyBody.AddRectCollider();

            AddTag("Bullet");

            MyBody.PassThrough = true;

        }

        public override void Initialize()
        {
            this.Transient = true;
        }


        public override void Update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);

            this.Transform.Translate(0, dir * 400 * Bootstrap.GetDeltaTime());

            Bootstrap.GetDisplay().DrawLine(
                (int)Transform.X,
                (int)Transform.Y,
                (int)Transform.X,
                (int)Transform.Y + 20,
                col);




        }

        public void OnCollisionEnter(PhysicsBody x)
        {
            GameSpaceInvaders g;

            if (x.Parent.CheckTag(destroyTag) == true || x.Parent.CheckTag("BunkerBit"))
            {
                ToBeDestroyed = true;
                x.Parent.ToBeDestroyed = true;

                if (x.Parent.CheckTag("Player"))
                {
                    g = (GameSpaceInvaders)Bootstrap.GetRunningGame();

                    g.Dead = true;
                }

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
            return "Bullet: " + Transform.X + ", " + Transform.X;
        }
    }
}
