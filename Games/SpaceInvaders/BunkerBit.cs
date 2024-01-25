using Kintsugi.Core;
using Kintsugi.Physics;

namespace SpaceInvaders
{
    class BunkerBit : GameObject, ICollisionHandler
    {

        public override void Initialize()
        {


            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("bunkerBit.png");

            SetPhysicsEnabled();
            MyBody.AddRectCollider();

            AddTag("BunkerBit");

            MyBody.PassThrough = true;

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

        public override void Update()
        {


            Bootstrap.GetDisplay().AddToDraw(this);
        }
    }
}
