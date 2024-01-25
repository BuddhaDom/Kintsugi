/*
*
*   This is about a simple an input system as you can have, and it's horrible.
*       Only used for illustrative purposes.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;

namespace Kintsugi.Input
{
    public class InputBasic : InputSystem
    {
        public override void GetInput()
        {
            InputEvent ie;
            ConsoleKeyInfo cki;
            if (!Console.KeyAvailable)
            {
                return;
            }

            cki = Console.ReadKey(true);

            ie = new InputEvent
            {
                Key = cki.KeyChar
            };

            InformListeners(ie, "KeyDown");
            InformListeners(ie, "KeyUp");

            Debug.Log("Key is " + ie.Key);
        }
    }
}
