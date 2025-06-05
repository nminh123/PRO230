using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChampionPoolManager : MonoBehaviour
{
    public List<Champion> champions;
    public Dictionary<Champion, int> pool = new Dictionary<Champion, int>();

    void Start()
    {
        foreach (Champion champ in champions)
        {
            int copies = champ.cost switch
            {
                1 => 999,
                2 => 222,
                3 => 333,
                4 => 444,
                5 => 55,
                _ => 0
            };
            pool[champ] = copies;
        }
    }

    public Champion GetRandomChampion(int cost)
    {
        var available = pool.Where(p => p.Key.cost == cost && p.Value > 0).Select(p => p.Key).ToList();
        if (available.Count == 0) return null;
        Champion chosen = available[Random.Range(0, available.Count)];
        pool[chosen]--;
        return chosen;
    }

    public void ReturnChampion(Champion champ)
    {
        if (pool.ContainsKey(champ))
            pool[champ]++;
    }

    public Champion PeekRandomChampion(int cost)
    {
        List<Champion> available = pool.Keys.Where(c => c.cost == cost && pool[c] > 0).ToList();

        if (available.Count == 0) return null;

        return available[Random.Range(0, available.Count)];
    }

    public void ConsumeChampion(Champion champ)
    {
        if (pool.ContainsKey(champ) && pool[champ] > 0)
            pool[champ]--;
        else
            Debug.LogWarning("đã hết champ trong pool");
    }

}
