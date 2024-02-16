using Kintsugi.Audio;
using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Physics;
using SDL2;
using System.Drawing;

namespace GameTest
{
    class Spaceship : GameObject, IInputListener, ICollisionHandler
    {
        bool up, down, turnLeft, turnRight;

        EventDescription fireEvent;
        public override void Initialize()
        {
            fireEvent = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/Weapons/Pistol");
            this.Transform.X = 500.0f;
            this.Transform.Y = 500.0f;
            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("spaceship.png");


            Bootstrap.GetInput().AddListener(this);

            up = false;
            down = false;

            SetPhysicsEnabled();

            MyBody.Mass = 1;
            MyBody.MaxForce = 10;
            MyBody.AngularDrag = 0.01f;
            MyBody.Drag = 0f;
            MyBody.StopOnCollision = false;
            MyBody.ReflectOnCollision = false;
            MyBody.ImpartForce = false;
            MyBody.Kinematic = false;


            //           MyBody.PassThrough = true;
            //            MyBody.AddCircleCollider(0, 0, 5);
            //            MyBody.AddCircleCollider(0, 34, 5);
            //            MyBody.AddCircleCollider(60, 18, 5);
            //     MyBody.AddCircleCollider();

            MyBody.AddRectCollider();

            AddTag("Spaceship");


        }

        public void fireBullet()
        {
            Bullet b = new Bullet();

            b.setupBullet(this, this.Transform.Centre.X, this.Transform.Centre.Y);

            b.Transform.Rotate(this.Transform.Rotz);

            //fireEvent.CreateInstance().Start();
            fireEvent.PlayImmediate();
            //Bootstrap.GetSound().PlaySound("fire.wav");
        }

        public void HandleInput(InputEvent inp, string eventType)
        {



            if (eventType == "KeyDown")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    turnRight = true;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    turnLeft = true;
                }

            }
            else if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                {
                    up = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                {
                    down = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    turnRight = false;
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    turnLeft = false;
                }


            }



            if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    fireBullet();
                }
            }
        }

        public override void PhysicsUpdate()
        {

            if (turnLeft)
            {
                MyBody.AddTorque(-0.3f);
            }

            if (turnRight)
            {
                MyBody.AddTorque(0.3f);
            }

            if (up)
            {

                MyBody.AddForce(this.Transform.Forward, 0.5f);

            }

            if (down)
            {
                MyBody.AddForce(this.Transform.Forward, -0.2f);
            }


        }

        public override void Update()
        {
            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public void OnCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.CheckTag("Bullet") == false)
            {
                MyBody.DebugColor = Color.Red;
            }
        }

        public void OnCollisionExit(PhysicsBody x)
        {

            MyBody.DebugColor = Color.Green;
        }

        public void OnCollisionStay(PhysicsBody x)
        {
            MyBody.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
