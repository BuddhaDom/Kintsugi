using Kintsugi_Engine.Core;
using Kintsugi_Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Objects
{
    internal abstract class Actor : Unit
    {
        public event EventHandler OnUnitTurnEnd;

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
            OnUnitTurnEnd?.Invoke(this, EventArgs.Empty);
        }
    }
}
