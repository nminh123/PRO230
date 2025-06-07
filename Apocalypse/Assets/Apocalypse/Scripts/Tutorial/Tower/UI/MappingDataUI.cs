using System.Collections.Generic;
using Game.Tutorial.Turret;
using TMPro;
using UnityEngine;
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
        [SerializeField] private Slider cHealSlider, cRegenHealSlider, cDamageSlider, cAmmorSlider;
        [SerializeField] private Slider nHealSlider, nRegenHealSlider, nDamageSlider, nAmmorSlider;
        private TowerSO currentLevel, nextLevel;
        private int currentLevelNum, nextLevelNum;

        private void Start()
        {
            UpgradeVisual();
            OnUpgradeButtonClick();
        }

        private void OnEnable()
        {
            Events.Instance.PopupEnableEvent += DisplayPopup;
            Events.Instance.UpdateVisualEvent += UpgradeVisual;
        }

        private void OnDisable()
        {
            Events.Instance.PopupEnableEvent -= DisplayPopup;
            Events.Instance.UpdateVisualEvent -= UpgradeVisual;
        }

        private void OnUpgradeButtonClick()
        {
            upgradeButton.onClick.AddListener(() => TowerUpgrade.Instance.IteratorHotbar(TowerUpgrade.Instance.cpyRequirements));
        }

        private void DisplayPopup()
        {
            UpgradeVisual();
        }

        /// <summary>
        /// Chức năng của hàm như tên hàm -> cập nhật hình của Popup.
        /// </summary>
        public void UpgradeVisual()
        {
            UpdateCurrentLevelUI();
            UpdateNextLevelUI();
            Debug.Log($"Current Heal Slider value: {cHealSlider.value}, Current RegenHeal Slider value: {cRegenHealSlider.value}, Current Damage Slider value: {cDamageSlider.value}, Current Ammor Slider value: {cAmmorSlider.value}");
            Debug.Log($"Next Heal Slider value: {nHealSlider.value}, Next RegenHeal Slider value: {nRegenHealSlider.value}, Next Damage Slider value: {nDamageSlider.value}, Next Ammor Slider value: {nAmmorSlider.value}");
            int i = 0;
            //CoRegenHealion: i có thể bằng currentLevel.GetRequirement.Count() -> có vấn đề về UI không thể tự sinh ra.
            //Duyệt 3 danh sách icons, texts và requirement, map những data trong requirement với icons và texts.
            while (i < 2)
            {
                icons[i].sprite = TowerUpgrade.Instance.GetCurrentLevel.GetRequirements[i].item.image;
                texts[i].text = TowerUpgrade.Instance.GetCurrentLevel.GetRequirements[i].quantity.ToString();
                // TowerUpgrade.Instance.CheckQuantity(texts[i], currentLevel.GetRequirements[i].quantity, currentLevel.GetRequirements[i].item);
                i++;
            }
        }

        private void UpdateSlider(Slider _slider, float val) => _slider.value = val;

        /// <summary>
        /// Update UI slider
        /// </summary>
        private void UpdateCurrentLevelUI()
        {
            Debug.Log($"current level: {TowerUpgrade.Instance.GetCurrentLevel.GetLevel}");
            UpdateSlider(cHealSlider, TowerUpgrade.Instance.GetCurrentLevel.GetHealth);
            UpdateSlider(cRegenHealSlider, TowerUpgrade.Instance.GetCurrentLevel.GetRegenHeal);
            UpdateSlider(cDamageSlider, TowerUpgrade.Instance.GetCurrentLevel.GetDamage);
            UpdateSlider(cAmmorSlider, TowerUpgrade.Instance.GetCurrentLevel.GetAmmor);
            currentLevelText.text = content + TowerUpgrade.Instance.GetCurrentLevel.GetLevel.ToString();
        }

        /// <summary>
        /// Update UI slider
        /// </summary>
        private void UpdateNextLevelUI()
        {
            Debug.Log($"next level: {TowerUpgrade.Instance.GetNextLevel.GetLevel}");
            UpdateSlider(nHealSlider, TowerUpgrade.Instance.GetNextLevel.GetHealth);
            UpdateSlider(nRegenHealSlider, TowerUpgrade.Instance.GetNextLevel.GetRegenHeal);
            UpdateSlider(nDamageSlider, TowerUpgrade.Instance.GetNextLevel.GetDamage);
            UpdateSlider(nAmmorSlider, TowerUpgrade.Instance.GetNextLevel.GetAmmor);
            nextLevelText.text = content + TowerUpgrade.Instance.GetNextLevel.GetLevel.ToString();
        }

        /// <summary>
        /// Cập nhập data cho UI
        /// </summary>
        /// <param name="mapLevelData">level trong <see cref="MappingDataUI"/></param>
        /// <param name="levelData">level trong <see cref="TowerUpgrade"/></param>
        /// <param name="text">text</param>
        /// abandon code
        private void UpdateData(TowerSO mapLevelData, TowerSO levelData, TextMeshProUGUI text)
        {
            mapLevelData = levelData;
            Debug.Log($"Map level data {mapLevelData.GetLevel}, {mapLevelData.GetDamage}, {mapLevelData.GetAmmor}");
            foreach (var o in mapLevelData.GetRequirements)
            {
                Debug.Log("Requirement");
                Debug.Log(o);
            }
            text.text = content + levelData.GetLevel.ToString();
            //Initialize more data in tower so
        }

        public int GetCurrentLevelNumber() => currentLevelNum;
        public int GetNextLevelNumber() => nextLevelNum;
        public TowerSO GetCurrentLevelSO() => currentLevel;
        public TowerSO GetNextLevelSO() => nextLevel;
    }
}