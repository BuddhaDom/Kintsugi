/*
*
*   This manager class makes sure Update gets called when it should on all the game objects, 
*       and also handles the pre-physics and post-physics ticks.  It also deals with 
*       transient objects (like bullets) and removing destroyed game objects from the system.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Core
{
    /// <summary>
    /// Manages all <see cref="GameObject"/> objects present in the game.
    /// </summary>
    public class GameObjectManager
    {
        private static GameObjectManager me;
        List<GameObject> myObjects;

        private GameObjectManager()
        {
            myObjects = new List<GameObject>();
        }

        /// <summary>
        /// Get an existing game object manager instance if it exists, or create a new one.
        /// </summary>
        /// <returns>The instance of the game object.</returns>
        public static GameObjectManager GetInstance()
        {
            if (me == null)
            {
                me = new GameObjectManager();
            }

            return me;
        }

        /// <summary>
        /// Add a game object to this manager.
        /// </summary>
        /// <param name="gob">Game object to add.</param>
        public void AddGameObject(GameObject gob)
        {
            myObjects.Add(gob);

        }

        /// <summary>
        /// Remove a game object from this manager.
        /// </summary>
        /// <param name="gob">Game object to remove.</param>
        public void RemoveGameObject(GameObject gob)
        {
            myObjects.Remove(gob);
        }

        /// <summary>
        /// Destroy any objects cue to be destroyed. Run the update function on all game objects. Called every frame.
        /// </summary>
        public void Update()
        {
            for (int i = myObjects.Count - 1; i >= 0; i--)
            {
                if (myObjects[i].ToBeDestroyed)
                {
                    myObjects.RemoveAt(i);
                }

            }
            foreach (var gameObject in myObjects)
            {
                gameObject.Update();
            }
        }

    }
}
