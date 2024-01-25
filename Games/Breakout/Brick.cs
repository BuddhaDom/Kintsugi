
using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Physics;

namespace GameBreakout
{
    class Brick : GameObject, IInputListener, ICollisionHandler
    {
        private int health;

        public int Health { get => health; set => health = value; }

        public override void Initialize()
        {


            SetPhysicsEnabled();

            MyBody.Mass = 1;
            MyBody.Kinematic = true;

            MyBody.AddRectCollider();

            AddTag("Brick");

        }

        public void HandleInput(InputEvent inp, string eventType)
        {




        }


        public override void Update()
        {

            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("brick" + Health + ".png");

            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public void OnCollisionEnter(PhysicsBody x)
        {
            Health -= 1;

            if (Health <= 0)
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
            return "Brick: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
