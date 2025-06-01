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

        public delegate void PopupEnable(bool isEnable);
        public event PopupEnable PopupEnableEvent;
        public void InvokePopupEnableEvent(bool _)
        {
            PopupEnableEvent?.Invoke(_);
        }
    }
}