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

        public event Action UpdateVisualEvent;
        public void InvokeUpdateVisualEvent()
        {
            UpdateVisualEvent?.Invoke();
        }

        public delegate void WarningIcon(bool _);
        public event WarningIcon WarningIconEvent;
        public void InvokeWarningIconEvent(bool _)
        {
            WarningIconEvent?.Invoke(_);
        }
    }
}