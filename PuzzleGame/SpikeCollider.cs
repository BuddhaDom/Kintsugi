using Kintsugi.Collision;
using PuzzleGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame
{
    internal class SpikeCollider: GridlayerCollider
    {
        public override void OnTriggerCollision(Collider other)
        {
            base.OnTriggerCollision(other);
            if (other is TileObjectCollider c)
            {
                if (c.TileObject is MovementActor a)
                {
                    a.RemoveFromGrid();
                    Console.WriteLine("Kill " + a);
                }
            }
        }
    }
}
