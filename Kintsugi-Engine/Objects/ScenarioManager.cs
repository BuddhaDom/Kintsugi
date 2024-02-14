using Kintsugi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi_Engine.Objects
{
    public abstract class ScenarioManager
    {
        public event EventHandler OnTurnOrderFinished;

        private bool begun = false;
        public abstract void OnBeginScenario();
        public abstract void OnEndScenario();
        public void BeginScenario()
        {
            if (begun)
            {
                throw new InvalidOperationException("Scenario has already begun!");
            }
            begun = true;
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
            roundManager.AddToTurnOrder(c);
        }
        internal void NextRound(object? sender, EventArgs e)
        {
            roundManager.Begin();
        }

        internal void SetupAndBeginScenario()
        {
            roundManager.OnRoundFinished += NextRound;
            roundManager.Begin();
        }

        private RoundManager roundManager = new();

    }
}
