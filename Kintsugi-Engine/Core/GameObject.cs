/*
*
*   Anything that is going to be an interactable object in your game should extend from GameObject.  
*       It handles the life-cycle of the objects, some useful general features (such as tags), and serves 
*       as the convenient facade to making the object work with the physics system.  It's a good class, Bront.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Physics;

namespace Kintsugi.Core
{
    public class GameObject
    {
        private Transform3D transform;
        private bool transient;
        private bool toBeDestroyed;
        private bool visible;
        private PhysicsBody myBody;
        private List<string> tags;

        public void AddTag(string str)
        {
            if (tags.Contains(str))
            {
                return;
            }

            tags.Add(str);
        }

        public void RemoveTag(string str)
        {
            tags.Remove(str);
        }

        public bool CheckTag(string tag)
        {
            return tags.Contains(tag);
        }

        public string GetTags()
        {
            string str = "";

            foreach (string s in tags)
            {
                str += s;
                str += ";";
            }

            return str;
        }

        public void SetPhysicsEnabled()
        {
            MyBody = new PhysicsBody(this);
        }


        public bool QueryPhysicsEnabled()
        {
            if (MyBody == null)
            {
                return false;
            }
            return true;
        }

        public Transform3D Transform
        {
            get => transform;
        }

        public Transform Transform2D
        {
            get => transform;
        }


        public bool Visible
        {
            get => visible;
            set => visible = value;
        }
        public bool Transient { get => transient; set => transient = value; }
        public bool ToBeDestroyed { get => toBeDestroyed; set => toBeDestroyed = value; }
        public PhysicsBody MyBody { get => myBody; set => myBody = value; }

        public virtual void Initialize()
        {
        }

        public virtual void Update()
        {

        }

        public virtual void PhysicsUpdate()
        {
        }

        public virtual void PrePhysicsUpdate()
        {
        }

        public GameObject()
        {
            GameObjectManager.GetInstance().AddGameObject(this);

            transform = new Transform3D(this);
            visible = false;

            ToBeDestroyed = false;
            tags = new List<string>();

            Initialize();

        }

        public void CheckDestroyMe()
        {

            if (!transient)
            {
                return;
            }

            if (Transform.X > 0 && Transform.X < Bootstrap.GetDisplay().GetWidth())
            {
                if (Transform.Y > 0 && Transform.Y < Bootstrap.GetDisplay().GetHeight())
                {
                    return;
                }
            }


            ToBeDestroyed = true;

        }

        public virtual void KillMe()
        {
            PhysicsManager.GetInstance().RemovePhysicsObject(myBody);

            myBody = null;
            //transform = null;
        }


    }
}
