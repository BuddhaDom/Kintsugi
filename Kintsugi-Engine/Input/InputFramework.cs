/*
*
*   SDL provides an input layer, and we're using that.  This class tracks input, anchors it to the 
*       timing of the game loop, and converts the SDL events into one that is more abstract so games 
*       can be written more interchangeably.
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Core;
using SDL2;

namespace Kintsugi.Input
{

    // We'll be using SDL2 here to provide our underlying input system.
    public class InputFramework : InputSystem
    {

        double tick, timeInterval;
        public override void GetInput()
        {

            SDL.SDL_Event ev;
            int res;
            InputEvent ie;

            tick += Bootstrap.GetDeltaTime();

            if (tick < timeInterval)
            {
                return;
            }

            while (tick >= timeInterval)
            {

                res = SDL.SDL_PollEvent(out ev);


                if (res != 1)
                {
                    return;
                }

                ie = new InputEvent();

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                {
                    SDL.SDL_MouseMotionEvent mot;

                    mot = ev.motion;

                    ie.X = mot.x;
                    ie.Y = mot.y;

                    InformListeners(ie, "MouseMotion");
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;

                    InformListeners(ie, "MouseDown");
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                {
                    SDL.SDL_MouseButtonEvent butt;

                    butt = ev.button;

                    ie.Button = butt.button;
                    ie.X = butt.x;
                    ie.Y = butt.y;

                    InformListeners(ie, "MouseUp");
                }

                if (ev.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                {
                    SDL.SDL_MouseWheelEvent wh;

                    wh = ev.wheel;

                    ie.X = (int)wh.direction * wh.x;
                    ie.Y = (int)wh.direction * wh.y;

                    InformListeners(ie, "MouseWheel");
                }


                if (ev.type == SDL.SDL_EventType.SDL_KEYDOWN)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    Debug.Log("Keydown: " + ie.Key);
                    InformListeners(ie, "KeyDown");
                }

                if (ev.type == SDL.SDL_EventType.SDL_KEYUP)
                {
                    ie.Key = (int)ev.key.keysym.scancode;
                    InformListeners(ie, "KeyUp");
                }

                tick -= timeInterval;
            }


        }

        public override void Initialize()
        {
            tick = 0;
            timeInterval = 1.0 / 60.0;
        }

    }
}