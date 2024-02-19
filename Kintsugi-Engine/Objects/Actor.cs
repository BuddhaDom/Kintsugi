using Kintsugi.Core;
using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Objects
{
    internal abstract class Actor : TileObject
    {
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
        }

        // This should be called by the developer.
        public void EndTurn()
        {
            OnEndTurn();
            OnActorTurnEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}
