using Kintsugi.Audio;
using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Events;
using Kintsugi.Objects.Properties;
using TacticsGameTest.Units;

namespace PuzzleGame
{
    internal class SpikeCollider: GridLayerCollider
    {
        public override void OnTriggerCollision(Collider other)
        {
            base.OnTriggerCollision(other);
            if (other is TileObjectCollider c)
            {
                if (c.TileObject is CombatActor a)
                {
                    var fireEvent = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/MeleeAttack");
                    fireEvent.PlayImmediate();
                    a.TakeDamage(1, 0);
                    Console.WriteLine("Kill " + a);
                }
            }
        }
    }
}
