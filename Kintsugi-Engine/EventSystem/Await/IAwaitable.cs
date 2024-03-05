using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.EventSystem.Await
{
    public interface IAwaitable
    {
        public abstract bool IsFinished();
    }
}
