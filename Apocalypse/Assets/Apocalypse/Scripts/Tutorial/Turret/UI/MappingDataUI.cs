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
        [SerializeField] private Button upgradeButton;
        private TowerSO currentLevel, nextLevel;
        private int SO_Level, currentLevelNum, nextLevelNum;

        private void Start()
        {
            UpdateData(TowerUpgrade.Instance.GetTowerSO);
            OnUpgradeButtonClick();
        }

        private void OnEnable()
        {
            Events.Instance.PopupEnableEvent += DisplayPopup;
            Events.Instance.UpdateVisualEvent += EventCallback;
        }

        private void OnDisable()
        {
            Events.Instance.PopupEnableEvent -= DisplayPopup;
            Events.Instance.UpdateVisualEvent -= EventCallback;
        }

        private void OnUpgradeButtonClick()
        {
            upgradeButton.onClick.AddListener(() => TowerUpgrade.Instance.IteratorHotbar(currentLevel.GetRequirement));
        }

        private void DisplayPopup()
        {
            UpdateData(TowerUpgrade.Instance.GetTowerSO);
            UpgradeVisual();
        }

        public void UpgradeVisual()
        {
            int i = 0;
            //Condition: i có thể bằng currentLevel.GetRequirement.Count() -> có vấn đề về UI không thể tự sinh ra.
            //Duyệt 3 danh sách icons, texts và requirement, map những data trong requirement với icons và texts.
            while (i < 2)
            {
                icons[i].sprite = currentLevel.GetRequirement[i].item.image;
                texts[i].text = currentLevel.GetRequirement[i].quantity.ToString();
                TowerUpgrade.Instance.CheckQuantity(texts[i], currentLevel.GetRequirement[i].quantity, currentLevel.GetRequirement[i].item);
                i++;
            }
        }

        private void EventCallback(int nCurrentLevel, int nNextLevel)
        {
            currentLevel = Resources.Load<TowerSO>(TowerUpgrade.Instance.path + "tower_" + nCurrentLevel);
            nextLevel = Resources.Load<TowerSO>(TowerUpgrade.Instance.path + "tower_" + nNextLevel);
            DisplayPopup();
        }

        private void UpdateData(TowerSO SO)
        {
            currentLevel = SO;
            SO_Level = SO.GetLevel;
            //Initialize more data in tower so
        }

        public int GetCurrentLevelNumber() => currentLevelNum;
        public int GetNextLevelNumber() => nextLevelNum;
        public TowerSO GetCurrentLevelSO() => currentLevel;
        public TowerSO GetNextLevelSO() => nextLevel;
    }
}