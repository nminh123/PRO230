using Game.Tutorial;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePopupUI : Singleton<LevelCompletePopupUI>
{
    [Header("UI References")]
    public GameObject popupPanel;
    public GameObject[] starIcons;

    public ItemRewardByLevelPopup[] rewardSlots;

    public Button homeButton;
    public Button replayButton;
    public Button nextButton;

    private void Start()
    {
        homeButton.onClick.AddListener(OnClickHome);
        replayButton.onClick.AddListener(OnClickReplay);
        nextButton.onClick.AddListener(OnClickNext);
    }

    public void Show(int starsEarned, (ItemSO item, int quantity)[] rewards)
    {
        popupPanel.SetActive(true);

        for (int i = 0; i < starIcons.Length; i++)
        {
            starIcons[i].SetActive(i < starsEarned);
        }


        foreach (var slot in rewardSlots)
        {
            slot.rewardImage.transform.parent.gameObject.SetActive(false);
        }

        for (int i = 0; i < rewards.Length && i < rewardSlots.Length; i++)
        {
            rewardSlots[i].rewardImage.sprite = rewards[i].item.image;
            rewardSlots[i].rewardText.text = rewards[i].quantity.ToString();

            rewardSlots[i].rewardImage.transform.parent.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }

    public void OnClickHome()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }

    public void OnClickReplay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickNext()
    {
        //LevelManager.SaveLevelProgress(LevelManager.currentLevel, starsEarned: 3); // Có thể nhận vào nếu cần
        //LevelManager.currentLevel++;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
    }
}
