using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RTSGameEvent
{
    public string eventName;
    public float triggerTime;       
    public float triggerChance = 1f;  // Tỷ lệ xảy ra, từ 0.0 - 1.0
    public string description;
    public System.Action triggerAction;
    public bool triggered = false;
}

