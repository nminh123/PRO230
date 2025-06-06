using System;
using UnityEngine;

namespace Game.Tutorial.Turret
{
    public class Events
    {
        public static Events Instance { get; } = new Events();

        public delegate void Upgrade(Sprite sprite);
        public event Upgrade UpgradeEvent;
        public void InvokeUpgradeEvent(Sprite sprite)
        {
            UpgradeEvent?.Invoke(sprite);
        }

        public event Action PopupEnableEvent;
        public void InvokePopupEnableEvent()
        {
            PopupEnableEvent?.Invoke();
        }

        public delegate void UpdateVisual(int nCurrentLevel, int nNextLevel);
        public event UpdateVisual UpdateVisualEvent;
        public void InvokeUpdateVisualEvent(int nCurrentLevel, int nNextLevel)
        {
            UpdateVisualEvent?.Invoke(nCurrentLevel, nNextLevel);
        }

        public delegate void WarningIcon(bool _);
        public event WarningIcon WarningIconEvent;
        public void InvokeWarningIconEvent(bool _)
        {
            WarningIconEvent?.Invoke(_);
        }
    }
}