/*
*
*   The Shard Physics Manager.   
*   
*   As with the PhysicsBody class, upon which this class depends, I make no claims as to the 
*       accuracy of the physics.  My interest in this course is showing you how an engine is 
*       architected.  It's not a course on game physics.  The task of making this work in 
*       a way that simulates real world physics is well beyond the scope of the course. 
*       
*   This class is responsible for a lot.  It handles the broad phase collision 
*       detection (via Sweep and Prune).  It handles the narrow phase collisions, making use of the 
*       collider objects and the Minkowski differences they generate.  It does some collision resolutions 
*       that are linked to the mass of colliding bodies.  And it has the management routines that 
*       let all that happen.
*       
*   @author Michael Heron
*   @version 1.0

*   Several substantial contributions to the code made by others:
*   @author Mårten Åsberg (see Changelog for 1.0.1)
*
*   
*/

using Kintsugi.Core;
using Kintsugi.Physics.Colliders;
using System.Globalization;
using System.Numerics;

namespace Kintsugi.Physics
{
    /*
     * An internal class used to hold a combination of two potentially colliding objects. 
     */

    internal class CollidingObject
    {
        PhysicsBody a, b;

        public CollidingObject()
        {
        }

        public CollidingObject(PhysicsBody x, PhysicsBody y)
        {
            A = x;
            B = y;
        }

        internal PhysicsBody A { get => a; set => a = value; }
        internal PhysicsBody B { get => b; set => b = value; }

        public override bool Equals(object other)
        {
            return other is CollidingObject co &&
                (A == co.A && B == co.B ||
                A == co.B && B == co.A);
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }

        public override string ToString()
        {
            return "[" + A.Parent.ToString() + " v " + B.Parent.ToString() + "]";
        }
    }

    /*
     * SAP is Search and Prune, a broad pass collision detection algorithm and the one used
     * in the course.   This is an internal class used to contain a node in what will be a 
     * linked list used elsewhere in this code.
     */

    public class SAPEntry
    {
        PhysicsBody owner;
        float start, end;
        SAPEntry previous, next;

        public float Start { get => start; set => start = value; }
        public float End { get => end; set => end = value; }
        internal PhysicsBody Owner { get => owner; set => owner = value; }
        internal SAPEntry Previous { get => previous; set => previous = value; }
        internal SAPEntry Next { get => next; set => next = value; }
    }


    public class PhysicsManager
    {
        private static PhysicsManager me;
        private List<CollidingObject> collisionsToCheck;
        HashSet<CollidingObject> colliding;

        private long timeInterval;
        SAPEntry sapX, sapY;
        float gravityModifier;
        Vector2 gravityDir;

        List<PhysicsBody> allPhysicsObjects;
        private long lastUpdate;
        private long lastDebugDraw;
        private PhysicsManager()
        {
            string tmp = "";
            string[] tmpbits;

            allPhysicsObjects = new List<PhysicsBody>();
            colliding = new HashSet<CollidingObject>();

            lastUpdate = Bootstrap.GetCurrentMillis();

            collisionsToCheck = new List<CollidingObject>();

            gravityDir = new Vector2(0, 1);
            // 50 FPS            

            TimeInterval = 20;

            if (Bootstrap.CheckEnvironmentalVariable("gravity_modifier"))
            {
                gravityModifier = float.Parse
                    (Bootstrap.GetEnvironmentalVariable("gravity_modifier"), CultureInfo.InvariantCulture);
            }
            else
            {
                gravityModifier = 0.1f;
            }

            if (Bootstrap.CheckEnvironmentalVariable("gravity_dir"))
            {
                tmp = Bootstrap.GetEnvironmentalVariable("gravity_dir");

                tmpbits = tmp.Split(",");

                gravityDir = new Vector2(int.Parse(tmpbits[0]), int.Parse(tmpbits[1]));
            }
            else
            {
                gravityDir = new Vector2(0, 1);
            }


        }

        public static PhysicsManager GetInstance()
        {
            if (me == null)
            {
                me = new PhysicsManager();
            }

            return me;
        }


        public long LastUpdate { get => lastUpdate; set => lastUpdate = value; }
        public long TimeInterval { get => timeInterval; set => timeInterval = value; }
        public long LastDebugDraw { get => lastDebugDraw; set => lastDebugDraw = value; }
        public float GravityModifier { get => gravityModifier; set => gravityModifier = value; }

        public void AddPhysicsObject(PhysicsBody body)
        {
            if (allPhysicsObjects.Contains(body))
            {
                return;
            }

            allPhysicsObjects.Add(body);

        }

        public void RemovePhysicsObject(PhysicsBody body)
        {
            allPhysicsObjects.Remove(body);
        }

        public void ClearList(SAPEntry node)
        {
            //Let's clear everything so the garbage collector can do its
            // work

            if (node == null)
            {
                return;
            }

            while (node != null && node.Next != null)
            {
                node = node.Next;
                node.Previous.Next = null;
                node.Previous = null;
            }

            node.Previous = null;

        }

        public SAPEntry AddToList(SAPEntry node, SAPEntry entry)
        {
            SAPEntry current;

            current = node;


            // Start our list.
            if (current == null)
            {
                return entry;
            }

            // Is this our new head?
            if (entry.Start < current.Start)
            {
                current.Previous = entry;
                entry.Next = current;
                return entry;
            }

            // Look for where we get inserted.
            while (current.Next != null && entry.Start > current.Next.Start)
            {
                current = current.Next;
            }


            if (current.Next != null)
            {
                // Insert ourselves into a chain.
                entry.Previous = current;
                entry.Next = current.Next;
                current.Next = entry;
            }
            else
            {
                // We're at the end.
                current.Next = entry;
                entry.Previous = current;
            }


            return node;

        }

        public void OutputList(SAPEntry node)
        {
            SAPEntry pointer = node;
            int counter = 0;
            string text = "";


            if (pointer == null)
            {
                Debug.Log("No List");
                return;
            }

            while (pointer != null)
            {
                text += "[" + counter + "]: " + pointer.Owner.Parent + ", ";
                pointer = pointer.Next;
                counter += 1;
            }

            Debug.Log("List:" + text);

        }

        public bool WillTick()
        {
            if (Bootstrap.GetCurrentMillis() - lastUpdate > TimeInterval)
            {
                return true;
            }

            return false;
        }

        public bool Update()
        {
            ICollisionHandler ch, ch2;
            List<CollidingObject> toRemove;

            if (WillTick() == false)
            {
                return false;
            }

            //            Debug.Log("Tick: " + Bootstrap.TimeElapsed);

            lastUpdate = Bootstrap.GetCurrentMillis();


            toRemove = new List<CollidingObject>();

            foreach (PhysicsBody body in allPhysicsObjects)
            {

                if (body.UsesGravity)
                {
                    body.ApplyGravity(gravityModifier, gravityDir);
                }

                body.PhysicsTick();
                body.RecalculateColliders();


            }


            // Check for old collisions that should be persisted
            foreach (CollidingObject col in colliding)
            {
                ch = (ICollisionHandler)col.A.Parent;
                ch2 = (ICollisionHandler)col.B.Parent;
                Vector2? impulse;

                // If the object has been destroyed in the interim, it should still 
                // trigger a collision exit.
                if (col.A.Parent.ToBeDestroyed)
                {
                    ch2.OnCollisionExit(null);
                    toRemove.Add(col);
                }

                if (col.B.Parent.ToBeDestroyed)
                {
                    ch.OnCollisionExit(null);
                    toRemove.Add(col);
                }

                impulse = CheckCollisionBetweenObjects(col.A, col.B);

                if (impulse != null)
                {
                    ch.OnCollisionStay(col.B);
                    ch2.OnCollisionStay(col.A);
                }
                else
                {
                    ch.OnCollisionExit(col.B);
                    ch2.OnCollisionExit(col.A);
                    toRemove.Add(col);
                }

            }

            foreach (CollidingObject col in toRemove)
            {
                colliding.Remove(col);
            }

            toRemove.Clear();
            // Check for new collisions
            CheckForCollisions();



            //            Debug.Log("Time Interval is " + (Bootstrap.GetCurrentMillis() - lastUpdate) + ", " + colliding.Count);


            return true;
        }

        public void DrawDebugColliders()
        {
            foreach (PhysicsBody body in allPhysicsObjects)
            {
                // Debug drawing - always happens.
                body.DrawMe();
            }
        }

        private static Vector2? CheckCollisionBetweenObjects(PhysicsBody a, PhysicsBody b)
        {
            Vector2? impulse;

            foreach (Collider col in a.GetColliders())
            {
                foreach (Collider col2 in b.GetColliders())
                {
                    impulse = col.CheckCollision(col2);


                    if (impulse != null)
                    {
                        return impulse;
                    }
                }
            }

            return null;

        }

        // omg this won't scale omg
        private void BroadPassBruteForce()
        {
            CollidingObject tmp;

            if (allPhysicsObjects.Count < 2)
            {
                // Nothing to collide.
                return;
            }

            for (int i = 0; i < allPhysicsObjects.Count; i++)
            {
                for (int j = 0; j < allPhysicsObjects.Count; j++)
                {

                    if (i == j)
                    {
                        continue;
                    }

                    if (FindColliding(allPhysicsObjects[i], allPhysicsObjects[j]))
                    {
                        continue;
                    }

                    if (FindColliding(allPhysicsObjects[j], allPhysicsObjects[i]))
                    {
                        continue;
                    }

                    tmp = new CollidingObject
                    {
                        A = allPhysicsObjects[i],
                        B = allPhysicsObjects[j]
                    };

                    collisionsToCheck.Add(tmp);

                }
            }

            //            Debug.Log("Checking " + collisionsToCheck.Count + " collisions");

        }

        public bool FindColliding(PhysicsBody a, PhysicsBody b)
        {
            CollidingObject col = new CollidingObject(a, b);

            return colliding.Contains(col);
        }

        private void NarrowPass()
        {
            Vector2 impulse;
            Vector2? possibleImpulse;
            float massTotal, massa, massb;
            float massProp = 0.0f;

            //            Debug.GetInstance().Log("Active objects " + collisionsToCheck.Count);

            foreach (CollidingObject ob in collisionsToCheck)
            {

                possibleImpulse = CheckCollisionBetweenObjects(ob.A, ob.B);

                if (possibleImpulse.HasValue)
                {
                    impulse = possibleImpulse.Value;
                    Debug.Log("Col is " + ob + ", impulse " + possibleImpulse);

                    if (ob.A.PassThrough != true && ob.B.PassThrough != true)
                    {


                        massTotal = ob.A.Mass + ob.B.Mass;

                        if (ob.A.Kinematic)
                        {
                            massProp = 1;
                        }
                        else
                        {
                            massProp = ob.A.Mass / massTotal;

                        }


                        if (ob.A.ImpartForce)
                        {
                            ob.A.ImpartForces(ob.B, massProp);
                            ob.A.ReduceForces(1.0f - massProp);
                        }

                        massb = massProp;

                        if (ob.B.Kinematic == false)
                        {
                            ob.B.Parent.Transform.Translate(-1 * (impulse.X * massProp), -1 * (impulse.Y * massProp));
                        }


                        if (ob.B.Kinematic)
                        {
                            massProp = 1;
                        }
                        else
                        {
                            massProp = 1.0f - massProp;
                        }

                        massa = massProp;


                        if (ob.A.Kinematic == false)
                        {

                            ob.A.Parent.Transform.Translate(impulse.X * massProp, impulse.Y * massProp);
                        }


                        if (ob.A.StopOnCollision)
                        {
                            ob.A.StopForces();
                        }

                        if (ob.B.StopOnCollision)
                        {
                            ob.B.StopForces();
                        }


                    }


                    ((ICollisionHandler)ob.A.Parent).OnCollisionEnter(ob.B);
                    ((ICollisionHandler)ob.B.Parent).OnCollisionEnter(ob.A);
                    colliding.Add(ob);



                    if (ob.A.ReflectOnCollision)
                    {
                        ob.A.ReflectForces(impulse);
                    }
                    if (ob.B.ReflectOnCollision)
                    {
                        ob.B.ReflectForces(impulse);
                    }


                }


            }
        }

        public void ReportCollisionsInAxis(SAPEntry start)
        {
            List<SAPEntry> activeObjects;
            List<int> toRemove;
            CollidingObject col;
            activeObjects = new List<SAPEntry>();
            toRemove = new List<int>();
            col = new CollidingObject();



            while (start != null)
            {

                activeObjects.Add(start);


                for (int i = 0; i < activeObjects.Count; i++)
                {

                    if (start == activeObjects[i])
                    {
                        continue;
                    }

                    if (start.Start >= activeObjects[i].End)
                    {
                        toRemove.Add(i);
                    }
                    else
                    {
                        col = new CollidingObject();

                        if (start.Owner.Mass > activeObjects[i].Owner.Mass)
                        {
                            col.A = start.Owner;
                            col.B = activeObjects[i].Owner;
                        }
                        else
                        {
                            col.B = start.Owner;
                            col.A = activeObjects[i].Owner;
                        }

                        if (!FindColliding(col.A, col.B))
                        {
                            collisionsToCheck.Add(col);
                        }
                        // Debug.GetInstance().Log("Adding potential collision: " + col.ToString());

                    }


                }


                for (int j = toRemove.Count - 1; j >= 0; j--)
                {
                    activeObjects.RemoveAt(toRemove[j]);
                }

                toRemove.Clear();

                start = start.Next;

            }

            //            Debug.Log("Checking " + collisionsToCheck.Count + " collisions");

        }


        public void BroadPassSearchAndSweep()
        {
            SAPEntry sx, sy;
            float[] x, y;
            sapX = null;
            sapY = null;
            List<PhysicsBody> candidates = new List<PhysicsBody>();


            foreach (PhysicsBody body in allPhysicsObjects)
            {
                sx = new SAPEntry();

                x = body.MinAndMaxX;

                sx.Owner = body;
                sx.Start = x[0];
                sx.End = x[1];


                sapX = AddToList(sapX, sx);

            }

            //            OutputList (sapX);
            // What we have at this point is a sorted linked list of all
            // our objects in order.  So now we go over them all to see 
            // what are viable collision candidates.  If they don't overlap 
            // in the axis, they can't collide so don't bother checking them.

            // Now we find all the candidates that overlap in 
            // the Y axis from those that overlap in the X axis.
            // A two pass sweep and prune.

            ReportCollisionsInAxis(sapX);
            ClearList(sapX);

        }
        public void BroadPass()
        {
            BroadPassSearchAndSweep();
            //          BroadPassBruteForce();
        }



        private void CheckForCollisions()
        {
            BroadPass();
            NarrowPass();

            collisionsToCheck.Clear();


        }

    }
}
