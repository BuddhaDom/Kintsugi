using Kintsugi.Audio;
using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.EventSystem.Events;
using Kintsugi.Objects.Properties;

namespace PuzzleGame
{
    internal class SpikeCollider: GridLayerCollider
    {
        public override void OnTriggerCollision(Collider other)
        {
            base.OnTriggerCollision(other);
            if (other is TileObjectCollider c)
            {
                if (c.TileObject is MovementActor a)
                {
                    var fireEvent = ((SoundFMOD)Bootstrap.GetSound()).LoadEventDescription("event:/Timbral");
                    fireEvent.PlayImmediate();
                    EventManager.I.Queue(
                        new BlockQueueForSeconds(1f
                        ));
                    EventManager.I.Queue(
                        new ActionEvent(
                            () => LevelManager.Instance.ResetLevel()
                            ));
                    Console.WriteLine("Kill " + a);
                }
            }
        }
    }
}
