using System;
using UnityEngine;

public class ChampionUnit : MonoBehaviour
{
    public Champion data;

    public void Initialize(Champion champ)
    {
        data = champ;
        // Setup sprite, stats, skill từ dữ liệu ScriptableObject nếu cần
    }

    internal void Setup(Champion champion)
    {

    }

}
