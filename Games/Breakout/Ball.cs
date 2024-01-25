using Kintsugi.Core;
using Kintsugi.Physics;
using System.Numerics;

namespace GameBreakout
{
    class Ball : GameObject, ICollisionHandler
    {
        float cx, cy;
        Vector2 dir, lastDir;
        internal Vector2 LastDir { get => lastDir; set => lastDir = value; }
        internal Vector2 Dir { get => dir; set => dir = value; }

        public override void Initialize()
        {


            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("ball.png");
            SetPhysicsEnabled();


            MyBody.AddCircleCollider();

            MyBody.Mass = 1;
            MyBody.MaxForce = 15;
            MyBody.Drag = 0f;
            MyBody.UsesGravity = false;
            MyBody.StopOnCollision = false;
            MyBody.ReflectOnCollision = true;

            Transform.Scalex = 2;
            Transform.Scaley = 2;


            Transform.Rotate(90);


        }

        public override void Update()
        {
            //            Debug.Log ("" + this);

            Bootstrap.GetDisplay().AddToDraw(this);

        }

        public void OnCollisionStay(PhysicsBody other)
        {
        }

        public void OnCollisionEnter(PhysicsBody other)
        {

            if (other.Parent.CheckTag("Paddle"))
            {
                //                            Debug.Log ("Hit the Paddle");
                Dir = new Vector2(Transform.Centre.X - other.Trans.Centre.X, LastDir.Y * -1);
            }

            if (other.Parent.CheckTag("Brick"))
            {
                //                            Debug.Log("Hit the Brick");

                //                            Dir = new Shard.Vector();
                //                            Dir.X = (float)(Transform.Centre.X - other.Trans.Centre.X);
                //                            Dir.Y = (float)(Transform.Centre.Y - other.Trans.Centre.Y);

            }



        }





        public void changeDir(int x, int y)
        {
            if (Dir == Vector2.Zero)
            {
                dir = lastDir;
            }

            if (x != 0)
            {
                dir = new Vector2(x, dir.Y);
            }

            if (y != 0)
            {
                dir = new Vector2(dir.X, y);
            }

        }


        public override void PhysicsUpdate()
        {


            if (Transform.Centre.Y - Transform.Ht <= 0)
            {
                changeDir(0, 1);
                Transform.Translate(0, -1 * Transform.Centre.Y);

                Debug.Log("Top wall");
            }

            if (Transform.Centre.Y + Transform.Ht >= Bootstrap.GetDisplay().GetHeight())
            {
                changeDir(0, -1);
                Transform.Translate(0, Transform.Centre.Y - Bootstrap.GetDisplay().GetHeight());

                Debug.Log("Bottom wall");

            }


            if (Transform.Centre.X - Transform.Wid <= 0)
            {
                changeDir(1, 0);
                Transform.Translate(-1 * Transform.Centre.X, 0);

                Debug.Log("Left wall");

            }

            if (Transform.Centre.X + Transform.Wid >= Bootstrap.GetDisplay().GetWidth())
            {
                changeDir(-1, 0);
                Transform.Translate(Transform.Centre.X - Bootstrap.GetDisplay().GetWidth(), 0);

                Debug.Log("Right wall");

            }

            if (Dir != Vector2.Zero)
            {

                Dir = Vector2.Normalize(Dir);

                if (Dir.Y > -0.2f && Dir.Y < 0)
                {
                    dir.Y = -0.2f;
                }
                else if (Dir.Y < 0.2f && Dir.Y >= 0)
                {
                    dir.Y = 0.2f;
                }

                if (Dir.X > -0.2f && Dir.X < 0)
                {
                    dir.X = -0.2f;
                }
                else if (Dir.X < 0.2f && Dir.X >= 0)
                {
                    dir.X = 0.2f;
                }

                MyBody.StopForces();
                MyBody.AddForce(Dir, 15);

                LastDir = Dir;
                dir = Vector2.Zero;
            }

        }
        public void OnCollisionExit(PhysicsBody x)
        {

        }


        public override string ToString()
        {
            return "Ball: [" + Transform.X + ", " + Transform.Y + ", Dir: " + Dir + ", LastDir: " + LastDir + ", " + Transform.Lx + ", " + Transform.Ly + "]";
        }


    }
}
