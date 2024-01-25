/*
*
*   Any game object interested in listening for input events will need to register itself 
*       with this manager.   It handles the informing of all listener objects when an 
*       event is raised.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Input
{

    public abstract class InputSystem
    {
        private List<IInputListener> myListeners;

        public virtual void Initialize()
        {
        }

        public InputSystem()
        {
            myListeners = new List<IInputListener>();
        }

        public void AddListener(IInputListener il)
        {
            if (myListeners.Contains(il) == false)
            {
                myListeners.Add(il);
            }
        }

        public void RemoveListener(IInputListener il)
        {
            myListeners.Remove(il);
        }

        public void InformListeners(InputEvent ie, string eventType)
        {
            IInputListener il;
            for (int i = 0; i < myListeners.Count; i++)
            {
                il = myListeners[i];

                if (il == null)
                {
                    continue;
                }

                il.HandleInput(ie, eventType);
            }
        }
        public abstract void GetInput();
    }
}
