using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Champion", menuName = "TFT/Champion")]
public class Champion : ScriptableObject
{
    public string championName;
    public int cost; // 1 to 5
    public Sprite icon;
}

