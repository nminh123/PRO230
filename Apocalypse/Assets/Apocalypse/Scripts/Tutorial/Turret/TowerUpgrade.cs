using System.Collections.Generic;
using Game.Tutorial;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Tutorials
{
    public class TowerUpgrade : MonoBehaviour
    {
        public static TowerUpgrade Instance;
        private readonly string path = "Scriptable Object/";

        [SerializeField] private TowerSO dataLevel;
        [SerializeField] private GameObject childrenIconLevelUp;
        [SerializeField] private Animator animator;
        public List<ItemRequirementToNextLevel> cpyRequirements;
        public bool IsTrigger { get; set; } = false;
        public bool isReadyToUpgrade { get; set; } = false;
        public bool isUpgrade { get; set; } = false;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            cpyRequirements = new List<ItemRequirementToNextLevel>(dataLevel.GetRequirement);
        }

        private void LateUpdate()
        {
            CheckItemToUpgrade(dataLevel.GetRequirement);
        }

        public TowerSO GetDataLevel() => dataLevel;

        //Code này được làm bởi nminh123 (28/05/2025)
        private void CheckItemToUpgrade(List<ItemRequirementToNextLevel> requirements)
        {
            if (IsTrigger)
            {
                IteratorHotbar(requirements, childrenIconLevelUp);
            }
            else
            {
                childrenIconLevelUp.SetActive(false);
            }
        }

        /// <summary>
        /// Hàm dùng để lặp hotbar slot, tìm xem có đủ item để nâng cấp ? nảy animation để báo player : không xảy ra gì.
        /// </summary>
        /// <param name="requirements">Những yêu cầu để nâng cấp thành</param>
        /// <param name="children">Object con để nhảy anim thông báo người chơi</param>
        public void IteratorHotbar(List<ItemRequirementToNextLevel> requirements, GameObject children)
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
                        isReadyToUpgrade = true;
                    }
                }
            }
        }

        /// <summary>
        /// Hàm dùng để lặp qua các hotbar slot, tìm xem có đủ item để nâng cấp không ? nếu người chơi click ở <seealso cref="TowerPointer"/> sẽ bắt click và <seealso cref="HotBarManager.TakeItem(ItemSO item, int quantityRequirement)"/>xoá item ở scene : không xảy ra gì.
        /// </summary>
        /// <param name="requirements">Những yêu cầu để nâng cấp thành.</param>
        public void IteratorHotbar(List<ItemRequirementToNextLevel> requirements)
        {
            var slots = HotBarManager.instance.hotBarSlots;
            for (int i = 0; i < requirements.Count; i++)
            {
                foreach (var slot in slots)
                {
                    HotBarItem item = slot.GetComponentInChildren<HotBarItem>();
                    if (item == null)
                    {
                        continue;
                    }
                    int itemCount = HotBarManager.instance.GetItemCount(item.itemSO);
                    if (item.itemSO.id == requirements[i].item.id && itemCount >= requirements[i].quantity)
                    {
                        //Xoá những item ở hotbar được hiển thị trên scene
                        HotBarManager.instance.TakeItem(item.itemSO, requirements[i].quantity);
                        cpyRequirements.Remove(requirements[i]);
                        NextLevel();
                    }
                }
            }
        }

        private void NextLevel()
        {
            if (cpyRequirements.Count == 0)
            {
                dataLevel = Resources.Load<TowerSO>(path + "tower_2");
            }
        }
    }
}