using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Tutorial;

namespace Game.Tutorials
{
    [CreateAssetMenu(fileName = "Tower", menuName = "TowerSO/Tower")]
    public class TowerSO : ScriptableObject
    {
        public TowerLevelData levels;

        public float getHealth => levels.health;
        public float getStVatLy => levels.stVatLy;
        public float getStPhep => levels.stPhep;
        public float getGiapVatLy => levels.giapVatLy;
        public float getGiapPhep => levels.giapPhep;
        public float getTocDoDanh => levels.tocDoDanh;
        public List<ItemRequirementToNextLevel> GetRequirement => levels.requrement;
    }

    [Serializable]
    public class TowerLevelData
    {
        public int level;
        public Sprite sprite;
        public float health;
        public float stVatLy;
        public float stPhep;
        public float giapVatLy;
        public float giapPhep;
        public float tocDoDanh;
        public List<ItemRequirementToNextLevel> requrement;
    }

    [Serializable]
    public class ItemRequirementToNextLevel
    {
        public ItemSO item;
        public int quantity;
    }
}