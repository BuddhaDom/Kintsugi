/*
*
*   Anything that is going to be an interactable object in your game should extend from GameObject.  
*       It handles the life-cycle of the objects, some useful general features (such as tags), and serves 
*       as the convenient facade to making the object work with the physics system.  It's a good class, Bront.
*   @author Michael Heron
*   @version 1.0
*   
*/
using System.Numerics;

namespace Kintsugi.Core
{
    public class GameObject
    {
        internal bool ToBeDestroyed;
        public Vector2 Position;
        public GameObject()
        {
            GameObjectManager.GetInstance().AddGameObject(this);
        }
        public virtual void Update() { }
        public void Destroy()
        {
            ToBeDestroyed = true;
        }
    }
}
