/*
*
*   Any object that will want to listen for input events should register that interest, for 
*       which they will need to have this interface implemented.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Input
{
    /// <summary>
    /// Interface to implement when listening for inputs.
    /// </summary>
    public interface IInputListener
    {
        /// <summary>
        /// Method ocurring when an input event is caught.
        /// </summary>
        /// <param name="inp">Input event.</param>
        /// <param name="eventType">Type of event.</param>
        public void HandleInput(InputEvent inp, string eventType);
    }
}
