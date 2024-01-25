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
    public interface IInputListener
    {


        public void HandleInput(InputEvent inp, string eventType);
    }
}
