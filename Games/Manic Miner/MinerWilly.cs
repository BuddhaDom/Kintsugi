using Kintsugi.Core;
using Kintsugi.Input;
using Kintsugi.Physics;
using SDL2;

namespace ManicMiner
{
    class MinerWilly : GameObject, IInputListener, ICollisionHandler
    {
        private string sprite;
        private bool left, right, jumpUp, jumpDown, fall, canJump;
        private int spriteCounter, spriteCounterDir;
        private string spriteName;
        private double spriteTimer, jumpCount;
        private double speed = 100, jumpSpeed = 260;
        private double fallCounter;

        public override void Initialize()
        {
            spriteName = "right";
            spriteCounter = 1;
            SetPhysicsEnabled();
            MyBody.AddRectCollider();
            AddTag("MinerWilly");
            spriteTimer = 0;
            jumpCount = 0;
            MyBody.Mass = 1;
            Bootstrap.GetInput().AddListener(this);


            Transform.Translate(0, 800);
            MyBody.StopOnCollision = false;
            MyBody.Kinematic = false;

            spriteCounterDir = 1;
        }


        public void HandleInput(InputEvent inp, string eventType)
        {
            if (eventType == "KeyDown")
            {

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = true;
                    spriteName = "right";

                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = true;
                    spriteName = "left";
                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE && canJump == true)
                {
                    jumpUp = true;
                    Debug.Log("Jumping up");
                }

            }

            else if (eventType == "KeyUp")
            {

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                {
                    right = false;

                }

                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                {
                    left = false;
                }



            }

        }

        public override void Update()
        {


            if (left)
            {
                this.Transform.Translate(-1 * speed * Bootstrap.GetDeltaTime(), 0);
                spriteTimer += Bootstrap.GetDeltaTime();
            }

            if (right)
            {
                this.Transform.Translate(1 * speed * Bootstrap.GetDeltaTime(), 0);
                spriteTimer += Bootstrap.GetDeltaTime();
            }

            if (jumpUp)
            {
                fall = false;
                fallCounter = 0;
                if (jumpCount < 0.3f)
                {
                    this.Transform.Translate(0, -1 * jumpSpeed * Bootstrap.GetDeltaTime());
                    jumpCount += Bootstrap.GetDeltaTime();
                }
                else
                {
                    jumpCount = 0;
                    jumpUp = false;
                    fall = true;

                }
            }



            if (spriteTimer > 0.1f)
            {
                spriteTimer -= 0.1f;
                spriteCounter += spriteCounterDir;

                if (spriteCounter >= 4)
                {
                    spriteCounterDir = -1;

                }

                if (spriteCounter <= 1)
                {
                    spriteCounterDir = 1;

                }


            }

            if (fall)
            {
                Transform.Translate(0, jumpSpeed * Bootstrap.GetDeltaTime());
                fallCounter += Bootstrap.GetDeltaTime();

                if (Transform.Y > 900)
                {
                    ToBeDestroyed = true;
                }

            }

            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath(spriteName + spriteCounter + ".png");


            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public bool shouldReset(PhysicsBody x)
        {
            float[] minAndMaxX = x.GetMinAndMax(true);
            float[] minAndMaxY = x.GetMinAndMax(false);

            if (Transform.X + Transform.Wid >= minAndMaxX[0] && Transform.X <= minAndMaxX[1])
            {
                // We're in the centre, so it's fine.

                if (Transform.Y + Transform.Ht <= minAndMaxY[0])
                {
                    return true;
                }

                if (Transform.Y >= minAndMaxY[1])
                {
                    jumpUp = false;
                    return false;
                }


            }

            return false;
        }

        public void OnCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.CheckTag("Collectible"))
            {
                return;
            }

            if (fallCounter > 2)
            {
                ToBeDestroyed = true;
            }

            fallCounter = 0;

            if (shouldReset(x))
            {
                fall = true;
            }
            else
            {
                fall = false;
            }

        }

        public void OnCollisionExit(PhysicsBody x)
        {
            if (x.Parent.CheckTag("Collectible"))
            {
                return;
            }

            Debug.Log("Falling: " + fall);
            canJump = false;
            fall = true;

        }

        public void OnCollisionStay(PhysicsBody x)
        {
            if (x.Parent.CheckTag("Collectible"))
            {
                return;
            }

            if (shouldReset(x))
            {
                fall = false;
                canJump = true;
                fallCounter = 0;
            }
            else
            {
                fall = true;
            }

        }
    }
}
