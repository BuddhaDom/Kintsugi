/*
*
*   The abstract collider class that is the base of all collisions.   Specific variations should
*       extend from this or one of its children.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using System.Drawing;
using System.Numerics;

namespace Kintsugi.Physics.Colliders
{
    public abstract class Collider
    {
        private ICollisionHandler gameObject;
        private float[] minAndMaxX;
        private float[] minAndMaxY;
        private bool rotateAtOffset;

        public abstract void Recalculate();
        public Collider(ICollisionHandler gob)
        {
            gameObject = gob;
            MinAndMaxX = new float[2];
            MinAndMaxY = new float[2];

        }

        internal ICollisionHandler GameObject { get => gameObject; set => gameObject = value; }
        internal float[] MinAndMaxX { get => minAndMaxX; set => minAndMaxX = value; }
        internal float[] MinAndMaxY { get => minAndMaxY; set => minAndMaxY = value; }
        public bool RotateAtOffset { get => rotateAtOffset; set => rotateAtOffset = value; }

        public abstract Vector2? CheckCollision(ColliderRect c);

        public abstract Vector2? CheckCollision(Vector2 c);

        public abstract Vector2? CheckCollision(ColliderCircle c);

        public virtual Vector2? CheckCollision(Collider c)
        {

            if (c is ColliderRect rect)
            {
                return CheckCollision(rect);
            }

            if (c is ColliderCircle circle)
            {
                return CheckCollision(circle);
            }

            Debug.Log("Bug");
            // Not sure how we got here but c'est la vie
            return null;
        }

        public abstract void DrawMe(Color col);

        public abstract float[] GetMinAndMaxX();
        public abstract float[] GetMinAndMaxY();

    }
}
