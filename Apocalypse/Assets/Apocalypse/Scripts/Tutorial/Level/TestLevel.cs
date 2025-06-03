using Game.Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLevel : MonoBehaviour
{
    [SerializeField] private ItemRewardByLevel[] itemRewards;
    public void OnLevelComplete(int starsEarned)
    {
        var rewards = new (ItemSO, int)[itemRewards.Length];
        for (int i = 0; i < itemRewards.Length; i++)
        {
            rewards[i] = (itemRewards[i].ItemSO, itemRewards[i].rewardAmounts);
        }

        LevelCompletePopupUI.Instance.Show(starsEarned, rewards);

        LevelManager.Instance.SaveLevelProgress(starsEarned);
        //SceneManager.LoadScene("Map"); 
    }
}