using Engine.EventSystem;
using Kintsugi.Collision;
using Kintsugi.Core;
using Kintsugi.EventSystem;
using Kintsugi.Objects.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticsGameTest.Units;


namespace TacticsGameTest.Events
{
    internal class PushedEvent : Kintsugi.EventSystem.Event
    {
        private SelectableActor actor;
        private Vec2Int direction;
        public PushedEvent(SelectableActor actor, Vec2Int direction)
        {
            this.direction = direction;
            this.actor = actor;
        }
        public override void OnExecute()
        {

            Vec2Int to = actor.Transform.Position + direction;
            var colliders = CollisionSystem.GetCollisionsColliderWithPosition(actor.Collider, actor.Transform.Grid, to);
            if (colliders.Count > 0)
            {
                actor.TakeDamage(1, 0);

                foreach (var col in colliders)
                {
                    if (col is TileObjectCollider tCol)
                    {
                        if (tCol.TileObject is SelectableActor act)
                        {
                            act.TakeDamage(1, 0);
                        }
                    }
                }
            }
            else
            {
                actor.PushTo(to);
            }
        }
    }
}
