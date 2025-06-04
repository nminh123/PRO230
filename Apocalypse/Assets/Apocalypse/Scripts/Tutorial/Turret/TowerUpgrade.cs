using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Tutorial.Turret
{
    public class TowerUpgrade : MonoBehaviour
    {
        public static TowerUpgrade Instance;
        public readonly string path = "Scriptable Object/";
        private readonly int maxLevel = 5;

        [SerializeField] private TowerSO towerSO;
        [SerializeField] private GameObject childrenIconLevelUp;
        [SerializeField] private Animator animator;
        private List<ItemSO> items = new List<ItemSO>();
        private int sumRequirements, level = 1, nextLevel;
        public List<ItemRequirementToNextLevel> cpyRequirements;
        public bool IsTrigger { get; set; } = false;
        public bool IsStayTrigger { get; set; } = false;
        public bool isReadyToUpgrade { get; set; } = false;
        public bool isUpgrade { get; set; } = false;

        private void Awake()
        {
            Instance = this;
            UpgradeLevel(level);
        }

        private void OnEnable()
        {
            Events.Instance.WarningIconEvent += WarningUpgrade;
        }

        private void OnDisable()
        {
            Events.Instance.WarningIconEvent -= WarningUpgrade;
        }

        //Code này được làm bởi nminh123 (28/05/2025)
        // private void CheckItemToUpgrade(List<ItemRequirementToNextLevel> requirements)
        // {
        //     if (IsTrigger && towerSO.GetLevel < maxLevel)
        //         IteratorHotbar(requirements, childrenIconLevelUp);
        //     else
        //         childrenIconLevelUp.SetActive(false);
        // }

        private void WarningUpgrade(bool _)
        {
            childrenIconLevelUp.SetActive(_);
            // animator.Play("WarningIcon");
        }

        /// <summary>
        /// Hàm dùng để lặp hotbar slot, tìm xem có đủ item để nâng cấp ? nảy animation để báo player : không xảy ra gì.
        /// </summary>
        /// <param name="requirements">Những yêu cầu để nâng cấp thành</param>
        /// <param name="children">Object con để nhảy anim thông báo người chơi</param>
        // public void IteratorHotbar(List<ItemRequirementToNextLevel> requirements, GameObject children)
        // {
        //     var slots = HotBarManager.instance.hotBarSlots;
        //     foreach (var requirement in requirements)
        //     {
        //         foreach (var slot in slots)
        //         {
        //             HotBarItem item = slot.GetComponentInChildren<HotBarItem>();
        //             if (item == null)
        //                 continue;
        //             int itemCount = HotBarManager.instance.GetItemCount(item.itemSO);
        //             if (item.itemSO.id == requirement.item.id && itemCount >= requirement.quantity)
        //             {
        //                 children.SetActive(true);
        //                 animator.Play("LevelUpArrowAnim");
        //                 isReadyToUpgrade = true;
        //             }
        //         }
        //     }
        // }

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
                            nextLevel = level + 1;
                            UpgradeLevel(level);
                            Events.Instance.InvokeUpgradeEvent(towerSO.GetSprite);
                            Events.Instance.InvokeUpdateVisualEvent(level, nextLevel);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Kiểm tra số lượng item và điều chỉnh màu của UI Popup.
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="quantityRequirement">số lượng</param>
        /// <param name="requirementItem">Item yêu cầu</param>
        public void CheckQuantity(TextMeshProUGUI text, int quantityRequirement, ItemSO requirementItem)
        {
            var slots = HotBarManager.instance.hotBarSlots;
            foreach (var slot in slots)
            {
                HotBarItem item = slot.GetComponentInChildren<HotBarItem>();
                if (item == null) continue;
                items.Add(item.itemSO);
                int itemCount = HotBarManager.instance.GetItemCount(item.itemSO);
                if (item.itemSO.id == requirementItem.id && itemCount >= quantityRequirement)
                {
                    text.color = Color.black;
                }
                else if (item.itemSO != requirementItem || itemCount < quantityRequirement || !items.Contains(requirementItem))
                {
                    text.color = Color.red;
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
        public int GetLevel => level;
    }
}