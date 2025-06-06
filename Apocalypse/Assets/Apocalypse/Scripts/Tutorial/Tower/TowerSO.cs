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
        public void SetLevel(int val) => levels.level = val;
        public float GetHealth => levels.health;
        public void SetHealth(float val) => levels.health = val;
        public float GetDamage => levels.Damage;
        public void SetDamage(float val) => levels.Damage = val;
        public float GetAmmor => levels.Ammor;
        public void SetAmmor(float val) => levels.Ammor = val;
        public float GetRegenHeal => levels.RegenHeal;
        public void SetRegenHeal(float val) => levels.RegenHeal = val;
        public List<ItemRequirementToNextLevel> GetRequirements => levels.requrement;
        public void SetRequirements(List<ItemRequirementToNextLevel> _) => levels.requrement = _;
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