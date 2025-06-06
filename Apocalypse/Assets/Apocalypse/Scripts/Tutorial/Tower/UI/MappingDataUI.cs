using System.Collections.Generic;
using Game.Tutorial.Turret;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Game.Tutorial.UI
{
    public class MappingDataUI : MonoBehaviour
    {
        private readonly string content = "Tower Level "; //Plus level
        [SerializeField] private List<Image> icons;
        [SerializeField] private List<TextMeshProUGUI> texts;
        [SerializeField] private TextMeshProUGUI currentLevelText, nextLevelText;
        [SerializeField] private Button upgradeButton;
        private TowerSO currentLevel, nextLevel;
        private int currentLevelNum, nextLevelNum;

        private void Awake()
        {
            // EventCallback(1, 2);
            // InitializeLevel();
        }

        private void Start()
        {
            UpdateData(TowerUpgrade.Instance.GetCurrentLevel, currentLevelText);
            UpdateData(nextLevel, nextLevelText);
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
            upgradeButton.onClick.AddListener(() => TowerUpgrade.Instance.IteratorHotbar(currentLevel.GetRequirements));
        }

        private void DisplayPopup()
        {
            UpdateData(TowerUpgrade.Instance.GetCurrentLevel, currentLevelText);
            UpdateData(nextLevel, nextLevelText);
            UpgradeVisual();
        }

        public void UpgradeVisual()
        {
            int i = 0;
            //Condition: i có thể bằng currentLevel.GetRequirement.Count() -> có vấn đề về UI không thể tự sinh ra.
            //Duyệt 3 danh sách icons, texts và requirement, map những data trong requirement với icons và texts.
            while (i < 2)
            {
                icons[i].sprite = currentLevel.GetRequirements[i].item.image;
                texts[i].text = currentLevel.GetRequirements[i].quantity.ToString();
                TowerUpgrade.Instance.CheckQuantity(texts[i], currentLevel.GetRequirements[i].quantity, currentLevel.GetRequirements[i].item);
                i++;
            }
        }

        private void EventCallback(int nCurrentLevel, int nNextLevel)
        {
            currentLevel = Resources.Load<TowerSO>(TowerUpgrade.Instance.path + "tower_" + nCurrentLevel);
            nextLevel = Resources.Load<TowerSO>(TowerUpgrade.Instance.path + "tower_" + nNextLevel);
            DisplayPopup();
        }

        private void InitializeLevel()
        {
            currentLevel = Resources.Load<TowerSO>(TowerUpgrade.Instance.path + "tower_" + 1);
            nextLevel = Resources.Load<TowerSO>(TowerUpgrade.Instance.path + "tower_" + 2);
        }

        private void UpdateData(TowerSO level, TextMeshProUGUI text)
        {
            currentLevel.SetLevel(level.GetLevel);
            currentLevel.SetHealth(level.GetHealth);
            currentLevel.SetDamage(level.GetDamage);
            currentLevel.SetAmmor(level.GetAmmor);
            currentLevel.SetRequirements(level.GetRequirements);
            text.text = content + level.GetLevel.ToString();
            //Initialize more data in tower so
        }

        public int GetCurrentLevelNumber() => currentLevelNum;
        public int GetNextLevelNumber() => nextLevelNum;
        public TowerSO GetCurrentLevelSO() => currentLevel;
        public TowerSO GetNextLevelSO() => nextLevel;
    }
}