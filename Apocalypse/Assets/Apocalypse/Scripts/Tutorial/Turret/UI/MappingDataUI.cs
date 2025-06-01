using System.Collections.Generic;
using Game.Tutorial.Turret;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tutorial.UI
{
    public class MappingDataUI : MonoBehaviour
    {
        [SerializeField] private List<Image> icons;
        [SerializeField] private List<TextMeshProUGUI> texts;
        private TowerSO currentLevel, nextLevel;
        private int level;

        private void Start()
        {
            UpdateData(TowerUpgrade.Instance.GetTowerSO);
        }

        private void OnEnable()
        {
            Events.Instance.PopupEnableEvent += EnablePopup;
        }

        private void OnDisable()
        {
            Events.Instance.PopupEnableEvent -= EnablePopup;
        }

        private void EnablePopup(bool isEnable)
        {
            UpdateData(TowerUpgrade.Instance.GetTowerSO);
            for (int i = 0; i < icons.Count;)
            {
                foreach (var requrement in currentLevel.GetRequirement)
                {
                    icons[i].sprite = requrement.item.image;
                    texts[i].text = requrement.quantity.ToString();
                    i++;
                }
            }
        }

        private void UpdateData(TowerSO SO)
        {
            currentLevel = SO;
            level = SO.GetLevel;
            //Initialize more data in tower so
        }
    }
}