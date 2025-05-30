using System;

namespace Game.Tutorial.Turret
{
    public class Events
    {
        public static Events Instance { get; } = new Events();

        public event Action UpgradeEvent;
        public void InvokeUpgradeEvent()
        {
            UpgradeEvent?.Invoke();
        }
    }
}