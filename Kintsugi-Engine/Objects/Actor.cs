using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Objects
{
    public abstract class Actor : TileObject
    {
        public bool InTurn { get; private set; } = false;
        public event EventHandler OnActorTurnEnd;

        public abstract void OnStartTurn();
        public abstract void OnEndTurn();
        public abstract void OnStartRound();
        public abstract void OnEndRound();
        internal void StartRound()
        {
            OnStartRound();
        }
        internal void EndRound()
        {
            OnEndRound();
        }
        internal void StartTurn()
        {
            OnStartTurn();
            InTurn = true;
        }
        internal void Update()
        {

        }

        // This should be called by the developer.
        public void EndTurn()
        {
            InTurn = false;
            OnEndTurn();
            OnActorTurnEnd?.Invoke(this, EventArgs.Empty);
        }

        public Actor(TileObjectTransform transform, TileObjectCollider? collider = null, TileObjectSprite? sprite = null) : base(transform, collider, sprite)
        {
        }

        public Actor() : this(new TileObjectTransform()) {}
    }
}
