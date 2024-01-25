using Kintsugi.Core;
using Kintsugi.Physics;
using System.Drawing;

namespace GameTest
{
    class Bullet : GameObject, ICollisionHandler
    {
        private Spaceship origin;

        public void setupBullet(Spaceship or, float x, float y)
        {
            this.Transform.X = x;
            this.Transform.Y = y;
            this.Transform.Wid = 10;
            this.Transform.Ht = 10;

            this.origin = or;

            SetPhysicsEnabled();

            MyBody.AddRectCollider((int)x, (int)y, 10, 10);

            AddTag("Bullet");

            //            MyBody.AddCircleCollider((int)x, (int)y, 5);

            MyBody.Mass = 100;
            MyBody.MaxForce = 50;
            //            MyBody.AddTorque(0.001f);

            MyBody.PassThrough = true;

        }

        public override void Initialize()
        {
            this.Transient = true;
        }

        public override void PhysicsUpdate()
        {
            MyBody.AddForce(this.Transform.Forward, 100.0f);
        }

        public override void Update()
        {
            Random r = new Random();
            Color col = Color.FromArgb(r.Next(0, 256), r.Next(0, 256), 0);


            Bootstrap.GetDisplay().DrawLine(
                (int)Transform.X,
                (int)Transform.Y,
                (int)Transform.X + 10,
                (int)Transform.Y + 10,
                col);

            Bootstrap.GetDisplay().DrawLine(
                (int)Transform.X + 10,
                (int)Transform.Y,
                (int)Transform.X,
                (int)Transform.Y + 10,
                col);


        }

        public void OnCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.CheckTag("Spaceship") == false)
            {
                Debug.Log("Boom! " + x);
                ToBeDestroyed = true;
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
