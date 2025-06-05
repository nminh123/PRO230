using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChampionOddsByLevel", menuName = "TFT/Odds By Level")]
public class ChampionOddsByLevel : ScriptableObject
{
    [System.Serializable]
    public class OddsPerLevel
    {
        public int level;
        public float[] odds; 
    }

    public List<OddsPerLevel> oddsPerLevelList;

    public float[] GetOddsForLevel(int level)
    {
        foreach (var entry in oddsPerLevelList)
        {
            if (entry.level == level)
                return entry.odds;
        }

        Debug.LogWarning("Không tìm thấy odds cho level " + level);
        return new float[] { 100, 0, 0, 0, 0 }; // fallback
    }
}
