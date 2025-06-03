using Game.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPopupUI : Singleton<LevelPopupUI>
{
    [Header("UI References")]
    public GameObject popupPanel;
    public TextMeshProUGUI stageText;
    public Image bannerImage;
    public TextMeshProUGUI missionLabel;
    public TextMeshProUGUI descriptionText;
    public ItemRewardByLevelPopup[] itemRewards;
    public Button startButton;

    [Header("Colors Difficulty")]
    public Color easyColor;
    public Color normalColor;
    public Color hardColor;

    private int selectedLevel;

    public void Show(int level, string description, (ItemSO item, int quantity)[] rewards, string difficulty)
    {
        selectedLevel = level;

        stageText.text = $"Stage {level + 1}";
        descriptionText.text = description;

        bannerImage.color = difficulty switch
        {
            "Easy" => easyColor,
            "Normal" => normalColor,
            "Hard" => hardColor,
            _ => normalColor
        };

        for (int i = 0; i < itemRewards.Length; i++)
        {
            itemRewards[i].rewardImage.gameObject.SetActive(false);
            itemRewards[i].rewardText.gameObject.SetActive(false);
        }

        for (int i = 0; i < rewards.Length && i < itemRewards.Length; i++)
        {
            itemRewards[i].rewardImage.sprite = rewards[i].item.image;
            itemRewards[i].rewardText.text = rewards[i].quantity.ToString();
            itemRewards[i].rewardImage.gameObject.SetActive(true);
            itemRewards[i].rewardText.gameObject.SetActive(true);
        }

        popupPanel.SetActive(true);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        LevelManager.currentLevel = selectedLevel;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map1");
    }
}
[System.Serializable]
public class ItemRewardByLevelPopup
{
    public Image rewardImage;
    public TextMeshProUGUI rewardText;
}
