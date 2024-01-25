using Kintsugi.Core;

namespace MissileCommand
{
    class ArsenalSprite : GameObject
    {

        public override void Initialize()
        {


            this.Transform.X = 200.0f;
            this.Transform.Y = 100.0f;
            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("missile.png");

        }

        public override void Update()
        {
            Bootstrap.GetDisplay().AddToDraw(this);
        }

    }
}
