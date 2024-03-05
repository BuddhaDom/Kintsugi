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
    public class GameObjectManager
    {
        private static GameObjectManager me;
        List<GameObject> myObjects;

        private GameObjectManager()
        {
            myObjects = new List<GameObject>();
        }

        public static GameObjectManager GetInstance()
        {
            if (me == null)
            {
                me = new GameObjectManager();
            }

            return me;
        }

        public void AddGameObject(GameObject gob)
        {
            myObjects.Add(gob);

        }

        public void RemoveGameObject(GameObject gob)
        {
            myObjects.Remove(gob);
        }

        public void Update()
        {
            foreach (var gameObject in myObjects)
            {
                gameObject.Update();
            }
        }

    }
}
