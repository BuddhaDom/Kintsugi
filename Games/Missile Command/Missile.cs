using Kintsugi.Core;
using Kintsugi.Physics;
using Kintsugi.Physics.Colliders;
using System.Drawing;
using System.Numerics;

namespace MissileCommand
{
    class Missile : GameObject, ICollisionHandler
    {
        private float targetx, targety;
        private float originx, originy;
        private Random rand;
        private bool mirv;
        private List<City> theTargets;
        private string targetTag;
        private ColliderCircle c;
        private int speed;
        private Color myColor;
        private double mirvCount;


        public string TargetTag { get => targetTag; set => targetTag = value; }

        public float Targetx { get => targetx; set => targetx = value; }
        public float Targety { get => targety; set => targety = value; }
        public float Originx { get => originx; set => originx = value; }
        public float Originy { get => originy; set => originy = value; }
        public bool Mirv { get => mirv; set => mirv = value; }
        internal List<City> TheTargets { get => theTargets; set => theTargets = value; }
        public int Speed { get => speed; set => speed = value; }
        public Color MyColor { get => myColor; set => myColor = value; }

        public override void Initialize()
        {
            rand = new Random();

            SetPhysicsEnabled();
            mirvCount = 0;

            //            MyBody.Kinematic = true;

            c = MyBody.AddCircleCollider((int)Transform.X, (int)Transform.Y, 2);

            MyBody.PassThrough = true;
        }


        public void boom()
        {
            Explosion exp;


            exp = new Explosion();
            exp.Transform.X = Transform.X;
            exp.Transform.Y = Transform.Y;
            exp.TargetTag = TargetTag;

            ToBeDestroyed = true;

        }

        public override void Update()
        {
            float xDist, yDist;
            List<City> remainingCities;
            int numWarheads = 3;
            Vector2 v;

            Color col = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));

            xDist = (float)(Targetx - Transform.X);
            yDist = (float)(Targety - Transform.Y);

            v = new(xDist, yDist);
            v = Vector2.Normalize(v);

            v.X = (float)(v.X * Bootstrap.GetDeltaTime() * Speed);
            v.Y = (float)(v.Y * Bootstrap.GetDeltaTime() * Speed);

            Transform.Translate(v);

            xDist = Math.Abs(xDist);
            yDist = Math.Abs(yDist);

            // Since we're not building a collider around a sprite or forces, we have to do this manually.
            c.X = (float)Transform.Centre.X;
            c.Y = (float)Transform.Centre.Y;
            c.Rad = 2;
            //            c.Recalculate();

            if (mirv)
            {
                if (mirvCount > 10)
                {
                    ToBeDestroyed = true;

                    remainingCities = new List<City>();
                    foreach (City c in TheTargets)
                    {
                        if (c != null && c.ToBeDestroyed == false)
                        {
                            remainingCities.Add(c);
                        }
                    }

                    if (remainingCities.Count < numWarheads)
                    {
                        numWarheads = remainingCities.Count;
                    }

                    for (int i = 0; i < numWarheads; i++)
                    {
                        Missile m = new Missile();
                        m.Originx = (float)this.Transform.X;
                        m.Originy = (float)this.Transform.Y;

                        m.Transform.Translate(m.Originx, m.Originy);

                        m.Targetx = remainingCities[i].Transform.Centre.X;
                        m.Targety = remainingCities[i].Transform.Centre.Y;

                        m.AddTag("EnemyMissile");

                        if (rand.Next(0, 100) < 10)
                        {
                            m.mirv = true;
                        }

                        m.TargetTag = "City";
                        m.Speed = Speed * 2;
                        m.MyColor = Color.Blue;
                        m.TheTargets = remainingCities;
                    }

                }
            }
            else if (xDist + yDist < Speed * Bootstrap.GetDeltaTime())
            {
                boom();
            }


            Bootstrap.GetDisplay().DrawLine((int)Originx, (int)Originy, (int)Transform.X, (int)Transform.Y, myColor);
            Bootstrap.GetDisplay().DrawCircle((int)Transform.X, (int)Transform.Y, 2, col);

            mirvCount += Bootstrap.GetDeltaTime();
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
            return "Missile: [" + Transform.X + ", " + Transform.Y + ", " + c.Rad + "]";
        }


    }
}
