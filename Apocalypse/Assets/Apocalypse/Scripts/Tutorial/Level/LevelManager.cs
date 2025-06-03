using TMPro;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Level[] levels;
    public static int currentLevel;
    public static int unlockedLevels;

    public TextMeshProUGUI totalStartText;

    public int totalStarts { get; private set; } = 0;

    void Start()
    {
        unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 0);
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].levelIndex = i;
            levels[i].LoadStars();
            levels[i].levelButton.interactable = (i <= unlockedLevels);
            totalStarts += levels[i].GetStar();
        }
        totalStartText.text = totalStarts.ToString();
    }

    public void OnClickLevel(int level)
    {
        currentLevel = level;
        //SceneManager.LoadScene("Map1");
    }

    public void SaveLevelProgress(int starsEarned)
    {
        Level current = levels[currentLevel];
        current.SaveStars(starsEarned);

        if (currentLevel == unlockedLevels)
        {
            unlockedLevels++;
            PlayerPrefs.SetInt("unlockedLevels", unlockedLevels);
        }
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        unlockedLevels = 0;

        foreach (Level level in levels)
        {
            level.ResetStars();
            level.levelButton.interactable = false;
        }

        levels[0].levelButton.interactable = true;
        totalStarts = 0;
        totalStartText.text = totalStarts.ToString();
    }
}
