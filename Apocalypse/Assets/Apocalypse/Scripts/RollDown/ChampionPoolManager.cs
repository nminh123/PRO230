using System.Collections;
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
            int copies = champ.cost switch {
                1 => 29, 2 => 22, 3 => 18, 4 => 12, 5 => 10, _ => 0
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
}
