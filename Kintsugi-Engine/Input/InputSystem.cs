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
    /// <summary>
    /// The input system to be implemented by the engine.
    /// </summary>
    public abstract class InputSystem
    {
        private List<IInputListener> myListeners;

        public bool IsHoveringCanvas { get; internal set; }

        /// <summary>
        /// How to initialize this Input System implementation.
        /// </summary>
        public virtual void Initialize()
        {
        }

        public InputSystem()
        {
            myListeners = new List<IInputListener>();
        }

        /// <summary>
        /// Add an input listener to the system.
        /// </summary>
        /// <param name="il">Input listener to add.</param>
        public void AddListener(IInputListener il)
        {
            if (myListeners.Contains(il) == false)
            {
                myListeners.Add(il);
            }
        }
        /// <summary>
        /// Clear input listeners in the collection of existing listeners.
        /// </summary>
        public void ClearListeners()
        {
            myListeners.Clear();
        }

        /// <summary>
        /// Remove an input listener from the collection.
        /// </summary>
        /// <param name="il">Which input listener to remove.</param>
        public void RemoveListener(IInputListener il)
        {
            myListeners.Remove(il);
        }

        /// <summary>
        /// Make input listeners handle inputs of this type.
        /// </summary>
        /// <param name="ie">The input event.</param>
        /// <param name="eventType">Its type.</param>
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
        /// <summary>
        /// Method to get the input of this input system implementation.
        /// </summary>
        public abstract void GetInput();
    }
}
