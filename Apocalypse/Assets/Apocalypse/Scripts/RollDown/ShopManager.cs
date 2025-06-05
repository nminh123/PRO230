using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopSlotUI
    {
        public Image iconImage;
        public TMP_Text nameText;
        public Image bgFrame;
        public GameObject overlay;
    }

    public List<ShopSlotUI> shopSlots;
    public ChampionPoolManager poolManager;
    public ChampionOddsByLevel oddsData;
    public Transform spawnPoint;
    public int playerLevel = 1;

    private Champion[] currentChampions = new Champion[5];

    void OnEnable()
    {
        StartCoroutine(DelayedRoll());
    }

    IEnumerator DelayedRoll()
    {
        yield return null; // đợi 1 frame để mọi UI được khởi tạo
        RollShop();
    }

    public void RollShop()
    {
        float[] odds = oddsData.GetOddsForLevel(playerLevel);

        for (int i = 0; i < shopSlots.Count; i++)
        {
            Champion rolled = RollByOdds(odds);
            currentChampions[i] = rolled;

            // Update UI
            shopSlots[i].iconImage.sprite = rolled.icon;
            shopSlots[i].nameText.text = rolled.championName;
            shopSlots[i].bgFrame.color = rolled.tierColor;
            shopSlots[i].overlay.SetActive(false);
        }
    }

    private Champion RollByOdds(float[] odds)
    {
        float rand = Random.Range(0f, 100f);
        float cumulative = 0f;

        for (int i = 0; i < odds.Length; i++)
        {
            cumulative += odds[i];
            if (rand <= cumulative)
            {
                return poolManager.PeekRandomChampion(i + 1);
            }
        }

        return null;
    }


    public void BuyChampion(int slotIndex)
    {
        Champion selected = currentChampions[slotIndex];
        if (selected == null) return;

        // Trừ số lượng tại đây
        poolManager.ConsumeChampion(selected);

        GameObject champ = Instantiate(selected.prefab, spawnPoint.position, Quaternion.identity);
        ChampionUnit unit = champ.GetComponent<ChampionUnit>();
        unit.Initialize(selected);

        shopSlots[slotIndex].overlay.SetActive(true);
        currentChampions[slotIndex] = null;
    }
    
}
