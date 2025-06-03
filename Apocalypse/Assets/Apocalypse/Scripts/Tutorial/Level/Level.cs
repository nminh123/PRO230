using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Tutorial;
using System;

public class Level : MonoBehaviour
{
    [Header("GUI")]
    public int levelIndex;
    public Button levelButton;
    public GameObject[] starIcons;

    [Header("SetUp Level")]
    public string description;
    public Difficulty difficulty;
    public ItemRewardByLevel[] itemRewardByLevels;

    private int star = 0;

    private void Start()
    {
        levelButton.onClick.AddListener(OnLevelClick);
    }

    private void OnLevelClick()
    {
        LevelPopupUI popup = LevelPopupUI.Instance;

        var rewards = new (ItemSO, int)[itemRewardByLevels.Length];
        for (int i = 0; i < itemRewardByLevels.Length; i++)
        {
            rewards[i] = (itemRewardByLevels[i].ItemSO, itemRewardByLevels[i].rewardAmounts);
        }

        popup.Show(levelIndex, description, rewards, difficulty.ToString());
    }

    private void UpdateStarUI()
    {
        for (int i = 0; i < starIcons.Length; i++)
        {
            if (starIcons[i] != null)
                starIcons[i].SetActive(i < star);
        }
    }

    public void LoadStars()
    {
        star = PlayerPrefs.GetInt($"level_{levelIndex}_stars", 0);
        UpdateStarUI();
    }

    public void SaveStars(int newStars)
    {
        if (newStars > star)
        {
            star = newStars;
            PlayerPrefs.SetInt($"level_{levelIndex}_stars", star);
            UpdateStarUI();
        }
    }

    public void ResetStars()
    {
        star = 0;
        PlayerPrefs.DeleteKey($"level_{levelIndex}_stars");
        UpdateStarUI();
    }

    public int GetStar() => star;
}
[Serializable]
public class ItemRewardByLevel
{
    public ItemSO ItemSO;
    public int rewardAmounts;
}
public enum Difficulty
{
    Easy,
    Normal,
    Hard
}