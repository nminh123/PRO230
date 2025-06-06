using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tutorial.Turret
{
    [CreateAssetMenu(fileName = "tower_", menuName = "Allies/Tower")]
    public class TowerSO : ScriptableObject
    {
        public TowerLevelData levels;

        public Sprite GetSprite => levels.sprite;
        public int GetLevel => levels.level;
        public float GetHealth => levels.health;
        public float GetDamage => levels.Damage;
        public float GetAmmor => levels.Ammor;
        public float GetRegenHeal => levels.RegenHeal;
        public List<ItemRequirementToNextLevel> GetRequirement => levels.requrement;
    }

    [Serializable]
    public class TowerLevelData
    {
        public int level;
        public Sprite sprite;
        public float health;
        public float Damage;
        public float Ammor;
        public float RegenHeal;//Toc do hoi mau
        public List<ItemRequirementToNextLevel> requrement;
    }

    [Serializable]
    public class ItemRequirementToNextLevel
    {
        public ItemSO item;
        public int quantity;
    }
}