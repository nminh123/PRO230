using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Tutorial.Turret
{
    public class TowerUpgrade : MonoBehaviour
    {
        public static TowerUpgrade Instance;
        private readonly string path = "Scriptable Object/";
        private readonly int maxLevel = 5;

        [SerializeField] private TowerSO towerSO;
        [SerializeField] private GameObject childrenIconLevelUp;
        [SerializeField] private Animator animator;
        private int sumRequirements, level = 1;
        public List<ItemRequirementToNextLevel> cpyRequirements;
        public bool IsTrigger { get; set; } = false;
        public bool isReadyToUpgrade { get; set; } = false;
        public bool isUpgrade { get; set; } = false;

        private void Awake()
        {
            Instance = this;
            UpgradeLevel(level);
        }

        private void LateUpdate()
        {
            CheckItemToUpgrade(towerSO.GetRequirement);
        }

        //Code này được làm bởi nminh123 (28/05/2025)
        private void CheckItemToUpgrade(List<ItemRequirementToNextLevel> requirements)
        {
            if (IsTrigger && towerSO.GetLevel < maxLevel)
                IteratorHotbar(requirements, childrenIconLevelUp);
            else
                childrenIconLevelUp.SetActive(false);
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
                        continue;
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
            foreach (var requirement in requirements)
            {
                foreach (var slot in slots)
                {
                    HotBarItem item = slot.GetComponentInChildren<HotBarItem>();
                    if (item == null)
                        continue;
                    int itemCount = HotBarManager.instance.GetItemCount(item.itemSO);
                    Debug.Log(itemCount);
                    if (item.itemSO.id == requirement.item.id && itemCount >= requirement.quantity)
                    {
                        Debug.Log($"item.ItemSO.{item.itemSO.id} == requirements.item.{requirement.item.id}");
                        if (requirement.quantity == 0)
                            continue;
                        //Xoá những item ở hotbar được hiển thị trên scene
                        HotBarManager.instance.TakeItem(item.itemSO, requirement.quantity);
                        sumRequirements -= requirement.quantity;
                        DisableItem(requirement);
                        if (sumRequirements == 0)
                        {
                            level++;
                            UpgradeLevel(level);
                            Events.Instance.InvokeUpgradeEvent(towerSO.GetSprite);
                        }
                    }
                }
            }
        }

        public void IteratorHotbar(List<ItemRequirementToNextLevel> requirements, TextMeshProUGUI text, Color color)
        {
            var slots = HotBarManager.instance.hotBarSlots;
            foreach (var requirement in requirements)
            {
                foreach (var slot in slots)
                {
                    HotBarItem item = slot.GetComponentInChildren<HotBarItem>();
                    if (item == null)
                        continue;
                    int itemCount = HotBarManager.instance.GetItemCount(item.itemSO);
                    Debug.Log(itemCount);
                    // if (item.itemSO.id == requirement.item.id && itemCount >= requirement.quantity)
                    // {
                    //     Debug.Log($"item.ItemSO.{item.itemSO.id} == requirements.item.{requirement.item.id}");
                    //     if (requirement.quantity == 0)
                    //         continue;
                    //     //Xoá những item ở hotbar được hiển thị trên scene
                    //     HotBarManager.instance.TakeItem(item.itemSO, requirement.quantity);
                    //     sumRequirements -= requirement.quantity;
                    //     DisableItem(requirement);
                    //     if (sumRequirements == 0)
                    //     {
                    //         level++;
                    //         UpgradeLevel(level);
                    //         Events.Instance.InvokeUpgradeEvent(towerSO.GetSprite);
                    //     }
                    // }
                    // if()
                }
            }
        }

        /// <summary>
        /// Hàm này dùng để nâng cấp level thành, dựa trên những scriptable object đã được tạo ở folder <see cref="path"/>
        /// </summary>
        /// <param name="level">level</param>
        private void UpgradeLevel(int level)
        {
            towerSO = Resources.Load<TowerSO>(path + "tower_" + level);
            cpyRequirements = towerSO.GetRequirement.Select(req => new ItemRequirementToNextLevel
            {
                item = req.item,
                quantity = req.quantity
            }).ToList();
            for (int i = 0; i < cpyRequirements.Count; i++)
            {
                sumRequirements += cpyRequirements[i].quantity;
            }
        }

        private void DisableItem(ItemRequirementToNextLevel item)
        {
            item.quantity = 0;
        }

        public TowerSO GetTowerSO => towerSO;
    }
}