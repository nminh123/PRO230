using System;
using Leo;

namespace Leo.Planting.Block
{
    [Serializable]
    public class BlockInfo
    {
        public bool isWatered;
        public GrowthStage currentStage;
    }
}