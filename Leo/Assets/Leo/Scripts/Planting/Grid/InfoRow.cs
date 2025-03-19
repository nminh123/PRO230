using System.Collections.Generic;
using System;
using Leo.Planting.Block;

namespace Leo.Planting.Grid
{
    [Serializable]
    public class InfoRow
    {
        public List<BlockInfo> blocks = new List<BlockInfo>();
    }
}