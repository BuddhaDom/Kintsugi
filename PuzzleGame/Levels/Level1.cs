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
           // scenario = new MovingScenario();
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


            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
                ActorFactory.Goal(game, grid),
            };
            scenario.goals[0].SetPosition(new Vec2Int(0, 1));
            scenario.goals[1].SetPosition(new Vec2Int(2, 0));

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

            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
                ActorFactory.Goal(game, grid),
            };
            scenario.goals[0].SetPosition(new Vec2Int(2, 0));
            scenario.goals[1].SetPosition(new Vec2Int(3, 0));

            group_player.AddActor(character);
            group_player.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
    internal class Level4 : Level
    {
        public override string GridPath => "Tiles\\level4.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var character = ActorFactory.Skeleton(game, grid);
            character.SetPosition(new Vec2Int(0, 1));

            var character2 = ActorFactory.Zombie(game, grid);
            character2.SetPosition(new Vec2Int(0, 3));

            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
                ActorFactory.Goal(game, grid),
            };
            scenario.goals[0].SetPosition(new Vec2Int(1, 1));
            scenario.goals[1].SetPosition(new Vec2Int(1, 3));

            group_player.AddActor(character);
            group_player.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
    internal class Level5 : Level
    {
        public override string GridPath => "Tiles\\level5.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var character = ActorFactory.Zombie(game, grid);
            character.SetPosition(new Vec2Int(0, 0));

            var character2 = ActorFactory.Zombie(game, grid);
            character2.SetPosition(new Vec2Int(6, 0));

            var character3 = ActorFactory.Ghost(game, grid);
            character3.SetPosition(new Vec2Int(0, 2));

            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
                ActorFactory.Goal(game, grid),
                ActorFactory.Goal(game, grid),
            };
            scenario.goals[0].SetPosition(new Vec2Int(0, 1));
            scenario.goals[1].SetPosition(new Vec2Int(3, 1));
            scenario.goals[2].SetPosition(new Vec2Int(5, 1));

            group_player.AddActor(character);
            group_player.AddActor(character2);
            group_player.AddActor(character3);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
    internal class Level6 : Level
    {
        public override string GridPath => "Tiles\\level6.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 15;
            Bootstrap.GetCameraSystem().Position = new Vector2(50, 50);

            var character = ActorFactory.Skeleton(game, grid);
            character.SetPosition(new Vec2Int(6, 6));

            var character2 = ActorFactory.Ghost(game, grid);
            character2.SetPosition(new Vec2Int(3, 3));

            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
                ActorFactory.Goal(game, grid),             
            };
            scenario.goals[0].SetPosition(new Vec2Int(0, 0));
            scenario.goals[1].SetPosition(new Vec2Int(3, 3));
         

            group_player.AddActor(character);
            group_player.AddActor(character2);

            scenario.AddControlGroup(group_player);
            // scenario.AddControlGroup(group2);

            scenario.BeginScenario();
        }
    }
    internal class Level7 : Level
    {
        public override string GridPath => "Tiles\\level7.tmx";

        public override void SetUp()
        {
            Bootstrap.GetCameraSystem().Size = 4 * 20;
            Bootstrap.GetCameraSystem().Position = new Vector2(70, 75);

            var character = ActorFactory.Zombie(game, grid);
            character.SetPosition(new Vec2Int(5, 4));

            var character2 = ActorFactory.Mummy(game, grid);
            character2.SetPosition(new Vec2Int(3, 4));

            scenario.goals = new List<Actor>()
            {
                ActorFactory.Goal(game, grid),
                ActorFactory.Goal(game, grid),
            };
            scenario.goals[0].SetPosition(new Vec2Int(0, 2));
            scenario.goals[1].SetPosition(new Vec2Int(8, 1));


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
            Bootstrap.GetCameraSystem().Size = 20 * 10;
            Bootstrap.GetCameraSystem().Position = new Vector2(150, 150);

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
