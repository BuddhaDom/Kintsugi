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
    /// <summary>
    /// The core representation of an "object" in the Kintsugi engine. Contains properties that allow it to be
    /// displayed, moved around, and destroyed.
    /// </summary>
    public class GameObject
    {
        internal bool ToBeDestroyed;
        /// <summary>
        /// This object's world space position.
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// Create an instance and add it to the <see cref="GameObjectManager"/>.
        /// </summary>
        public GameObject()
        {
            GameObjectManager.GetInstance().AddGameObject(this);
        }
        /// <summary>
        /// Method to be called every frame.
        /// </summary>
        public virtual void Update() { }
        /// <summary>
        /// Queue this object to be destroyed.
        /// </summary>
        public void Destroy()
        {
            ToBeDestroyed = true;
        }
    }
}
