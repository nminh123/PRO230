using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Tutorial.Headquarters
{
    public class HQUpgrade : MonoBehaviour
    {
        public static HQUpgrade Instance;
        private readonly string path = "Scriptable Object/Headquarters/";

        private HQSO currentLevel;
        private int level, nextLevel, sumRequirements;
        private List<HQItemRequirementToNextLevel> cpyRequirements;

        private void Awake()
        {
            Instance = this;
        }

        #region Upgrade
        /// <summary>
        /// Hàm này dùng để nâng cấp level thành, dựa trên những scriptable object đã được tạo ở folder <see cref="path"/>
        /// </summary>
        /// <param name="level">level</param>
        private void UpgradeLevel(int level)
        {
            currentLevel = Resources.Load<HQSO>(path + "hq_" + level);
            cpyRequirements = currentLevel.GetRequirement.Select(req => new HQItemRequirementToNextLevel
            {
                item = req.item,
                quantity = req.quantity
            }).ToList();
            for (int i = 0; i < cpyRequirements.Count; i++)
            {
                sumRequirements += cpyRequirements[i].quantity;
            }
        }

        public void IteratorHotbar(List<HQItemRequirementToNextLevel> requirements)
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
                            // Events.Instance.InvokeUpgradeEvent(towerSO.GetSprite);
                            // Events.Instance.InvokeUpdateVisualEvent(level, nextLevel);
                        }
                    }
                }
            }
        }

        private void DisableItem(HQItemRequirementToNextLevel item)
        {
            item.quantity = 0;
        }
        #endregion
    }
}