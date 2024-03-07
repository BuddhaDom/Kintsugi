/*
*
*   This is a general, simple container for all the information someone might want to know about 
*       keyboard or mouse input.   The same object is used for both, so use your common sense 
*       to work out whether you can use the contents of, say 'x' and 'y' when registering for 
*       a key event.
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Kintsugi.Input
{
    /// <summary>
    /// Representation of an input event.
    /// </summary>
    public class InputEvent
    {
        private int x;
        private int y;
        private int button;
        private int key;
        private string classification;

        public int X
        {
            get => x;
            set => x = value;
        }
        public int Y
        {
            get => y;
            set => y = value;
        }
        /// <summary>
        /// The button pressed.
        /// </summary>
        public int Button
        {
            get => button;
            set => button = value;
        }
        /// <summary>
        /// Category of the input.
        /// </summary>
        public string Classification
        {
            get => classification;
            set => classification = value;
        }
        /// <summary>
        /// Key pressed.
        /// </summary>
        public int Key
        {
            get => key;
            set => key = value;
        }
    }
}
