using Kintsugi.Core;

namespace Kintsugi.AI
{
    public class Path
    {
        internal Path(List<Vec2Int> pathPositions, float cost)
        {
            PathPositions = pathPositions;
            Cost = cost;
        }
        public float Cost { get; }
        public List<Vec2Int> PathPositions { get; }
    }
}
