using Kintsugi.Objects.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kintsugi.Collision
{
    public class Collider
    {
        public HashSet<string> BelongLayers { get; internal set; } = [];
        public HashSet<string> CollideLayers { get; internal set; } = [];
        public bool IsTrigger { get; internal set; }

        public virtual void OnTriggerCollision(Collider other)
        {
            Console.WriteLine("Trigger collision between " + this + " and " + other);
        }
    }
}
