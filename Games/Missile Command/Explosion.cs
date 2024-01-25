using Kintsugi.Core;
using Kintsugi.Physics;
using Kintsugi.Physics.Colliders;
using System.Drawing;

namespace MissileCommand
{
    class Explosion : GameObject, ICollisionHandler
    {
        private int maxRadius;
        private float currentRadius;
        private float radDir;
        private Random rand;
        private ColliderCircle c;
        private string targetTag;

        public string TargetTag { get => targetTag; set => targetTag = value; }
        public override void Initialize()
        {
            currentRadius = 0;
            radDir = 1f;
            maxRadius = 50;
            rand = new Random();

            SetPhysicsEnabled();

            c = MyBody.AddCircleCollider(0, 0, (int)currentRadius);

            MyBody.PassThrough = true;

        }

        public override void PrePhysicsUpdate()
        {


        }

        public override void Update()
        {
            Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));
            int explosionSpeed = maxRadius / 4;

            if (currentRadius >= maxRadius)
            {
                radDir *= -1;
            }

            currentRadius += (float)(radDir * Bootstrap.GetDeltaTime() * explosionSpeed);

            c.X = (float)Transform.Centre.X;
            c.Y = (float)Transform.Centre.Y;
            c.Rad = currentRadius;
            c.Recalculate();


            if (currentRadius <= -1)
            {
                ToBeDestroyed = true;
            }


            Bootstrap.GetDisplay().DrawFilledCircle((int)Transform.X, (int)Transform.Y, (int)currentRadius, col);

        }

        public void OnCollisionEnter(PhysicsBody x)
        {

            if (x.Parent.CheckTag(TargetTag))
            {
                Debug.Log("Collided!");
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
            return "Explosion: [" + c.Left + ", " + c.Right + ", " + c.Top + ", " + c.Bottom + "]";
        }


    }
}
