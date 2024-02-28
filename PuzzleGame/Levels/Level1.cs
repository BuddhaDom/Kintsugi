using Kintsugi.Audio;
using Kintsugi.Core;
using Kintsugi.Objects;
using Kintsugi.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.Levels
{
    internal class Level1 : Level
    {
        public override string GridPath => "Tiles\\level1.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            


            // TODO: This sucks. Should be attached to grid on the transform itself.
            var character = ActorFactory.Zombie(game, grid);
            character.SetPosition(new Vec2Int(0, 5));
            var scenario = new MovingScenario();
            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
            };
            scenario.goals[0].SetPosition(new Vec2Int(4, 0));
            group_player.AddActor(character);
            //group.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
    internal class Level2 : Level
    {
        public override string GridPath => "Tiles\\level2.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var character = ActorFactory.Zombie(game, grid);
            character.SetPosition(new Vec2Int(0, 4));

            var character2 = ActorFactory.Zombie(game, grid);
            character2.SetPosition(new Vec2Int(3, 4));

            var scenario = new MovingScenario();

            group_player.AddActor(character);
            group_player.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
    internal class Level3 : Level
    {
        public override string GridPath => "Tiles\\level3.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var character = ActorFactory.Zombie(game, grid);
            character.SetPosition(new Vec2Int(0, 5));

            var character2 = ActorFactory.Zombie(game, grid);
            character2.SetPosition(new Vec2Int(4, 5));

            var scenario = new MovingScenario();

            group_player.AddActor(character);
            group_player.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
    internal class cutscene_EasterEgg : Level
    {
        public override string GridPath => "Tiles\\cutscene_Easteregg.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var music = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/Music");
            music.PlayImmediate();


         

            for (var i = 0; i < 50; i++)
            {
                var character = ActorFactory.Zombie(game, grid);
                character.SetPosition(new Vec2Int(i*2, 0));
                group_player.AddActor(character);
                for (var j = 0; j < 50; j++)
                {
                    character = ActorFactory.Zombie(game, grid);
                    character.SetPosition(new Vec2Int(i*2, (j*2)));
                    group_player.AddActor(character);
                }
            }
            

            var scenario = new MovingScenario();

            
            

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
}
