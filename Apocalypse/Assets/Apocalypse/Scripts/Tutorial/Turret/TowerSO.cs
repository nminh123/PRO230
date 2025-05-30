using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Tutorial;

namespace Game.Tutorial.Turret
{
    [CreateAssetMenu(fileName = "Tower", menuName = "TowerSO/Tower")]
    public class TowerSO : ScriptableObject
    {
        public TowerLevelData levels;

        public Sprite GetSprite => levels.sprite;
        public float GetHealth => levels.health;
        public float GetStVatLy => levels.stVatLy;
        public float GetStPhep => levels.stPhep;
        public float GetGiapVatLy => levels.giapVatLy;
        public float GetGiapPhep => levels.giapPhep;
        public float GetTocDoDanh => levels.tocDoDanh;
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