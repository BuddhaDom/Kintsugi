/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Assets;
using Kintsugi.Audio;
using Kintsugi.EventSystem;
using Kintsugi.Input;
using Kintsugi.Rendering;

namespace Kintsugi.Core
{
    /// <summary>
    /// Point of access class for different systems and components of the Kintsugi engine.
    /// </summary>
    public class Bootstrap
    {
        public static readonly string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static DisplayBase displayEngine;
        private static Sound soundEngine;
        private static InputSystem input;
        private static AssetManagerBase asset;
        private static CameraSystem cameraSystem;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static double deltaTime;
        private static double timeElapsed;
        private static int frames;
        private static List<long> frameTimes;
        private static long startTime;
        private static string baseDir;
        private static Dictionary<string, string> enVars;

        /// <summary>
        /// Check if an environmental variable exists.
        /// </summary>
        /// <param name="id">ID of the variable.</param>
        /// <returns><c>true</c> if the ID exists.</returns>
        public static bool CheckEnvironmentalVariable(string id)
        {
            return enVars.ContainsKey(id);
        }

        /// <summary>
        /// Look up an environmental variable.
        /// </summary>
        /// <param name="id">ID of the variable.</param>
        /// <returns>The value attributed to that variable.</returns>
        public static string GetEnvironmentalVariable(string id)
        {
            if (CheckEnvironmentalVariable(id))
            {
                return enVars[id];
            }

            return null;
        }
        
        /// <summary>
        /// Time elapsed since the game launched.
        /// </summary>
        public static double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }

        /// <summary>
        /// Gets the base directory of the game.
        /// </summary>
        /// <returns>Path of the game's file.</returns>
        public static string GetBaseDir()
        {
            return baseDir;
        }

        /// <summary>
        /// Prepare the environmental variables and set up the game with them.
        /// </summary>
        public static void Setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = workDir;

            SetupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            Setup(baseDir + "\\" + DEFAULT_CONFIG);

        }
        
        /// <summary>
        /// Prepare environmental variables.
        /// </summary>
        /// <param name="path">Path to the config file.</param>
        public static void SetupEnvironmentalVariables(string path)
        {
            Console.WriteLine("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.ReadConfigFile(path);

            enVars = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> kvp in config)
            {
                enVars[kvp.Key] = kvp.Value;
            }
        }
        
        /// <summary>
        /// Get the time between last frame and this one.
        /// </summary>
        /// <returns>Time elapsed since last frame.</returns>
        public static double GetDeltaTime()
        {

            return deltaTime;
        }

        /// <summary>
        /// Fetches the display configured for the engine..
        /// </summary>
        /// <returns>The display used by the engine.</returns>
        public static DisplayBase GetDisplay()
        {
            return displayEngine;
        }

        /// <summary>
        /// Fetches the sound engine configured.
        /// </summary>
        /// <returns>The sound engine.</returns>
        public static Sound GetSound()
        {
            return soundEngine;
        }

        /// <summary>
        /// Fetches the input system configured.
        /// </summary>
        /// <returns>The input system.</returns>
        public static InputSystem GetInput()
        {
            return input;
        }

        /// <summary>
        /// Fetches the asset manager configured.
        /// </summary>
        /// <returns>The asset manager.</returns>
        public static AssetManagerBase GetAssetManager()
        {
            return asset;
        }

        /// <summary>
        /// Fetches the currently running game.
        /// </summary>
        /// <returns>The game.</returns>
        public static Game GetRunningGame()
        {
            return runningGame;
        }

        /// <summary>
        /// Fetches the camera system configured.
        /// </summary>
        /// <returns>The camera system.</returns>
        public static CameraSystem GetCameraSystem()
        {
            return cameraSystem;
        }
        
        /// <summary>
        /// Initial setup resulting from config file parameters.
        /// </summary>
        /// <param name="path">Path to the config file.</param>
        public static void Setup(string path)
        {
            Console.WriteLine("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.ReadConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            foreach (KeyValuePair<string, string> kvp in config)
            {
                t = Type.GetType("Kintsugi." + kvp.Value);

                if (t == null)
                {
                    Debug.GetInstance().Log("Missing Class Definition: " + kvp.Value + " in " + kvp.Key, Debug.DEBUG_LEVEL_ERROR);
                    Environment.Exit(0);
                }

                ob = Activator.CreateInstance(t);


                switch (kvp.Key)
                {
                    case "display":
                        displayEngine = (DisplayBase)ob;
                        displayEngine.Initialize();
                        break;
                    case "sound":
                        soundEngine = (Sound)ob;
                        soundEngine.Initialize();
                        break;
                    case "asset":
                        asset = (AssetManagerBase)ob;
                        asset.RegisterAssets();
                        break;
                    case "input":
                        input = (InputSystem)ob;
                        input.Initialize();
                        break;

                }

                Debug.Log("Config file... setting " + kvp.Key + " to " + kvp.Value);
            }

            cameraSystem = new CameraSystem(displayEngine);

            if (runningGame == null)
            {
                Debug.GetInstance().Log("No game set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (displayEngine == null)
            {
                Debug.GetInstance().Log("No Display engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (soundEngine == null)
            {
                Debug.GetInstance().Log("No sound engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (bailOut)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Gets the current system millisecond.
        /// </summary>
        /// <returns><see cref="DateTime.Now"/> in milliseconds.</returns>
        public static long GetCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Get frames per second the game is running at.
        /// </summary>
        /// <returns>Number of frames being displayed each second.</returns>
        public static int GetFPS()
        {
            int fps;
            double seconds;

            seconds = (GetCurrentMillis() - startTime) / 1000.0;

            fps = (int)(frames / seconds);

            return fps;
        }

        /// <summary>
        /// Alternative form of <see cref="GetFPS"/>
        /// </summary>
        /// <returns>Number of frames being displayed each second.</returns>
        public static int GetSecondFPS()
        {
            int count = 0;
            long now = GetCurrentMillis();
            int lastEntry;



            Debug.Log("Frametimes is " + frameTimes.Count);

            if (frameTimes.Count == 0)
            {
                return -1;
            }

            lastEntry = frameTimes.Count - 1;

            while (frameTimes[lastEntry] > now - 1000 && lastEntry > 0)
            {
                lastEntry -= 1;
                count += 1;
            }

            if (lastEntry > 0)
            {
                frameTimes.RemoveRange(0, lastEntry);
            }

            return count;
        }

        /// <summary>
        /// Gets the current frame.
        /// </summary>
        /// <returns>Number of the frame.</returns>
        public static int GetCurrentFrame()
        {
            return frames;
        }

        internal static void RunStuff(Game game)
        {

            runningGame = game;
            targetFrameRate = runningGame.GetTargetFrameRate();
            millisPerFrame = 1000 / targetFrameRate;


            // Setup the engine.
            Setup();

            // When we start the program running.
            startTime = GetCurrentMillis();
            frames = 0;
            frameTimes = new List<long>();
            // Start the game running.
            runningGame.Initialize();

            // This is our game loop.
            MainLoop();
        }

        /// <summary>
        /// Main iterative processes of the engine.
        /// </summary>
        static void MainLoop()
        {
            long timeInMillisecondsStart, lastTick, timeInMillisecondsEnd;
            long interval;
            int sleep;

            while (true)
            {
                frames += 1;

                timeInMillisecondsStart = GetCurrentMillis();

                // Clear the screen.
                GetDisplay().ClearDisplay();

                // Update 
                runningGame.Update();
                // Input

                if (runningGame.IsRunning() == true)
                {

                    // Get input, which works at 50 FPS to make sure it doesn't interfere with the 
                    // variable frame rates.
                    input.GetInput();

                    EventManager.I.ProcessQueue();

                    // Update runs as fast as the system lets it.  Any kind of movement or counter 
                    // increment should be based then on the deltaTime variable.
                    GameObjectManager.GetInstance().Update();
                    // Update the physics.  If it's too soon, it'll return false.   Otherwise 
                    // it'll return true.

                }

                // Update sound engine
                soundEngine.Update();

                // Render the screen.
                GetDisplay().Display();

                timeInMillisecondsEnd = GetCurrentMillis();

                frameTimes.Add(timeInMillisecondsEnd);

                interval = timeInMillisecondsEnd - timeInMillisecondsStart;

                sleep = (int)(millisPerFrame - interval);


                TimeElapsed += deltaTime;

                if (sleep >= 0)
                {
                    // Frame rate regulator.  Bear in mind since this is millisecond precision, and we 
                    // only get whole numbers from our interval, it will only rarely match a target 
                    // FPS.  Milliseconds just aren't precise enough.
                    //
                    //  (I'm hinting if this bothers you, you might have found an engine modification to make...)
                    Thread.Sleep(sleep);
                }

                timeInMillisecondsEnd = GetCurrentMillis();
                deltaTime = (timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f;

                millisPerFrame = 1000 / targetFrameRate;

                lastTick = timeInMillisecondsStart;

            }
        }
    }
}
