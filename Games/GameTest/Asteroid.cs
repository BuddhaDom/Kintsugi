using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Physics;
using System.Numerics;

namespace GameTest
{
    class Asteroid : GameObject, ICollisionHandler, IInputListener
    {
        int torqueCounter = 0;
        public void HandleInput(InputEvent inp, string eventType)
        {

            if (eventType == "MouseDown" && inp.Button == 2)
            {
                if (MyBody.CheckCollisions(new Vector2(inp.X, inp.Y)) != null)
                {
                    torqueCounter += 10;
                }
            }


        }

        public override void Initialize()
        {
            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("asteroid.png");

            SetPhysicsEnabled();

            MyBody.MaxTorque = 100;
            MyBody.Mass = 1;
            MyBody.AngularDrag = 0.0f;
            MyBody.MaxForce = 100;
            MyBody.UsesGravity = true;
            MyBody.StopOnCollision = false;
            MyBody.ReflectOnCollision = true;
            //            MyBody.Kinematic = true;


            MyBody.AddForce(this.Transform.Right, 20.5f);
            //            MyBody.AddCircleCollider(32, 32, 30);
            MyBody.AddRectCollider();
            Bootstrap.GetInput().AddListener(this);

            AddTag("Asteroid");

        }


        public override void PhysicsUpdate()
        {
            for (int i = 0; i < torqueCounter; i++)
            {
                MyBody.AddTorque(0.1f);
            }

            if (torqueCounter > 0)
            {
                torqueCounter -= 1;
            }



        }

        public override void Update()
        {
            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public void OnCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.CheckTag("Bullet") == true)
            {
                ToBeDestroyed = true;
                Debug.Log("Boom");
            }

            Debug.Log("Bang");

        }

        public void OnCollisionExit(PhysicsBody x)
        {
            Debug.Log("Anti Bang");
        }

        public void OnCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Asteroid: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }
    }
}
