using System.Collections.Generic;
using UnityEngine;

namespace Game.Tutorial.Headquarters
{
    [CreateAssetMenu(fileName = "hq_", menuName = "Allies/Headquarters")]
    public class HQSO : ScriptableObject
    {
        public HQLevelData LevelData;
        public int GetLevel => LevelData.Level;
        public float GetHP => LevelData.HP;
        public int GetCredit => LevelData.Credit;
        public float GetRegenRate => LevelData.RegenRate;
        public float GetAmmor => LevelData.Ammor;
        public float GetRollCost => LevelData.RollCost;
        public float GetTimeLeftToNextRoll => LevelData.TimeLeftToNextRoll;
        public List<HQItemRequirementToNextLevel> GetRequirement => LevelData.ItemRequirement;
    }

    [System.Serializable]
    public struct HQLevelData
    {
        public int Level;
        public float HP;
        public int Credit;
        public float RegenRate;//Tốc độ hồi máu.
        public float Ammor;
        public float RollCost;
        public float TimeLeftToNextRoll;
        public List<HQItemRequirementToNextLevel> ItemRequirement;
    }

    [System.Serializable]
    public struct HQItemRequirementToNextLevel
    {
        public ItemSO item;
        public int quantity;
    }
}