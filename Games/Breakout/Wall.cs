using Kintsugi.Core;
using Kintsugi.Physics;

namespace GameBreakout
{
    class Wall : GameObject, ICollisionHandler
    {

        public override void Initialize()
        {


            SetPhysicsEnabled();

            MyBody.AddRectCollider();

            MyBody.Mass = 10;


            AddTag("Wall");

            MyBody.Kinematic = true;

        }


        public override void Update()
        {


        }

        public void OnCollisionEnter(PhysicsBody x)
        {

        }

        public void OnCollisionExit(PhysicsBody x)
        {
        }

        public void OnCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Wall: [" + Transform.X + ", " + Transform.Y + "]";
        }


    }
}
