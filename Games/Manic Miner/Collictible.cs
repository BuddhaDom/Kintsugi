using Kintsugi.Core;
using Kintsugi.Physics;
using System.Drawing;

namespace ManicMiner
{
    class Collectible : GameObject, ICollisionHandler
    {

        public override void Initialize()
        {
            SetPhysicsEnabled();

            AddTag("Collectible");
            MyBody.AddRectCollider((int)Transform.X, (int)Transform.Y, 10, 10);
            MyBody.PassThrough = true;

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
            if (x.Parent.CheckTag("MinerWilly"))
            {
                this.ToBeDestroyed = true;
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
            return "Collectible: [" + Transform.X + ", " + Transform.Y + "]";
        }


    }
}
