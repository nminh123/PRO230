using UnityEngine;

[CreateAssetMenu(fileName = "Champion", menuName = "TFT/Champion")]
public class Champion : ScriptableObject
{
    public string championName;
    public int cost; // 1–5
    public Sprite icon;
    public GameObject prefab;
}
