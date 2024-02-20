using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem
{
    public abstract class Event
    {
        public abstract void Execute();
    }
}
