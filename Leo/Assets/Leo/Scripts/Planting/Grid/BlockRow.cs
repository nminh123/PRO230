using System.Collections.Generic;
using Leo.Planting.Block;

namespace Leo.Planting.Grid
{
    [System.Serializable]
    public class BlockRow
    {
        public List<GrowBlock> blocks = new List<GrowBlock>();
    }
}