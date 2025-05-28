using System.Collections.Generic;
using Game.Tutorial;
using UnityEngine;

namespace Game.Tutorials
{
    public class TowerUpgrade : MonoBehaviour
    {
        public static TowerUpgrade Instance;

        [SerializeField] private TowerSO dataLevel;
        [SerializeField] private GameObject children;
        [SerializeField] private Animator animator;
        // [SerializeField] private ItemSO go;
        public bool IsTrigger { get; set; } = false;

        public void Awake()
        {
            Instance = this;
        }

        // private void Start()
        // {
        //     Dictionary<ItemSO, int> items = new Dictionary<ItemSO, int>
        //     {
        //         { go, 1 }
        //     };
        //     var boola = HotBarManager.instance.TakeItem(items);
        //     Debug.Log(boola);
        // }

        private void LateUpdate()
        {
            CheckItemToUpgrade(dataLevel.GetRequirement);
        }

        //Code này được làm bởi nminh123 (28/05/2025)
        private void CheckItemToUpgrade(List<ItemRequirementToNextLevel> requirements)
        {
            if (IsTrigger)
            {
                var slots = HotBarManager.instance.hotBarSlots;
                foreach (var requirement in requirements)
                {
                    foreach (var slot in slots)
                    {
                        HotBarItem item = slot.GetComponentInChildren<HotBarItem>();
                        if (item == null)
                        {
                            continue;
                        }
                        int itemCount = HotBarManager.instance.GetItemCount(item.itemSO);
                        if (item.itemSO.id == requirement.item.id && itemCount >= requirement.quantity)
                        {
                            children.SetActive(true);
                            animator.Play("LevelUpArrowAnim");
                        }
                    }
                }
            }
            else
            {
                children.SetActive(false);
            }
        }
    }
}