/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

using Kintsugi.Assets;
using Kintsugi.Audio;
using Kintsugi.Input;
using Kintsugi.Physics;
using Kintsugi.Rendering;

namespace Kintsugi.Core
{
    public class Bootstrap
    {
        public static readonly string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static DisplayBase displayEngine;
        private static Sound soundEngine;
        private static InputSystem input;
        private static PhysicsManager phys;
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

        public static bool CheckEnvironmentalVariable(string id)
        {
            return enVars.ContainsKey(id);
        }


        public static string GetEnvironmentalVariable(string id)
        {
            if (CheckEnvironmentalVariable(id))
            {
                return enVars[id];
            }

            return null;
        }


        public static double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }

        public static string GetBaseDir()
        {
            return baseDir;
        }

        public static void Setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = Directory.GetParent(workDir).Parent.Parent.FullName; ;

            SetupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            Setup(baseDir + "\\" + DEFAULT_CONFIG);

        }

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
        public static double GetDeltaTime()
        {

            return deltaTime;
        }

        public static DisplayBase GetDisplay()
        {
            return displayEngine;
        }

        public static Sound GetSound()
        {
            return soundEngine;
        }

        public static InputSystem GetInput()
        {
            return input;
        }

        public static AssetManagerBase GetAssetManager()
        {
            return asset;
        }

        public static Game GetRunningGame()
        {
            return runningGame;
        }

        public static void Setup(string path)
        {
            Console.WriteLine("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.ReadConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            phys = PhysicsManager.GetInstance();

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

            cameraSystem = new CameraSystem()

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

        public static long GetCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static int GetFPS()
        {
            int fps;
            double seconds;

            seconds = (GetCurrentMillis() - startTime) / 1000.0;

            fps = (int)(frames / seconds);

            return fps;
        }

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


            phys.GravityModifier = 0.1f;
            // This is our game loop.
            MainLoop();




        }

        static void MainLoop()
        {
            long timeInMillisecondsStart, lastTick, timeInMillisecondsEnd;
            long interval;
            int sleep;
            bool physUpdate = false;
            bool physDebug = false;

            timeInMillisecondsStart = startTime;
            lastTick = startTime;

            if (GetEnvironmentalVariable("physics_debug") == "1")
            {
                physDebug = true;
            }


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

                    // Update runs as fast as the system lets it.  Any kind of movement or counter 
                    // increment should be based then on the deltaTime variable.
                    GameObjectManager.GetInstance().Update();

                    // This will Update every 20 milliseconds or thereabouts.  Our physics system aims 
                    // at a 50 FPS cycle.
                    if (phys.WillTick())
                    {
                        GameObjectManager.GetInstance().PrePhysicsUpdate();
                    }

                    // Update the physics.  If it's too soon, it'll return false.   Otherwise 
                    // it'll return true.
                    physUpdate = phys.Update();

                    if (physUpdate)
                    {
                        // If it did tick, give every object an Update
                        // that is pinned to the timing of the physics system.
                        GameObjectManager.GetInstance().PhysicsUpdate();
                    }

                    if (physDebug)
                    {
                        phys.DrawDebugColliders();
                    }

                }

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
