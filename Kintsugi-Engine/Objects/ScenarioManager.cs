using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi_Engine.Objects
{
    internal abstract class ScenarioManager
    {
        public event EventHandler OnTurnOrderFinished;

        public abstract void OnBeginScenario();
        public abstract void OnEndScenario();
        public void BeginScenario()
        {
            OnBeginScenario();
            SetupAndBeginScenario();
        }
        public void EndScenario()
        {
            OnEndScenario();
            OnTurnOrderFinished?.Invoke(this, EventArgs.Empty);
        }
        public void AddControlGroup(ControlGroup c)
        {
            turnOrder.AddToTurnOrder(c);
        }
        RoundManager turnOrder = new();
        internal void NextRound(object? sender, EventArgs e)
        {
            turnOrder.Begin();
        }

        internal void SetupAndBeginScenario()
        {
            turnOrder.OnRoundFinished += NextRound;
            turnOrder.Begin();
        }
    }
}
