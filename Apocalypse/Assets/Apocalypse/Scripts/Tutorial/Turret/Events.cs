using System;
using UnityEngine;

namespace Game.Tutorials
{
    public class Events
    {
        public static Events Instance { get; } = new Events();

        public event Action TurretLevelUpEvent;
        public void InvokeTurretLevelUpEvent()
        {
            Debug.Log("InvokeTurretLevelUpEvent called!");
            TurretLevelUpEvent?.Invoke();
        }

        public event Action CheckHotBarEvent;
        public void InvokeCheckHotBarEvent()
        {
            Debug.Log("You are so close to turret - checking hotbar");
            CheckHotBarEvent?.Invoke();
        }
    }
}