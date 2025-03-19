using System;
using UnityEngine;
using Global;
using FarmingGame.Planting.Block;

[Serializable]
public class BlockInfo
{
    public bool isWatered;
    public GrowthStage currentStage;
}