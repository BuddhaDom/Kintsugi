using Kintsugi.Core;
using Kintsugi.Physics;

namespace MissileCommand
{
    class City : GameObject, ICollisionHandler
    {

        public override void Initialize()
        {
            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("city.png");

            SetPhysicsEnabled();

            MyBody.AddCircleCollider(64, 64, 64);

            AddTag("City");

            MyBody.Kinematic = true;

        }


        public override void Update()
        {

            Bootstrap.GetDisplay().AddToDraw(this);

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
            return "City: [" + Transform.X + ", " + Transform.Y + "]";
        }


    }
}
