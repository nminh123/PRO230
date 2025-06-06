using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private float timer;
    public EventNotificationUI eventUI;
    public List<RTSGameEvent> eventList = new List<RTSGameEvent>();

    void Start()
    {
        InitializeEvents(); 
    }

    void Update()
    {
        timer += Time.deltaTime;

        foreach (var gameEvent in eventList)
        {
            if (!gameEvent.triggered && timer >= gameEvent.triggerTime)
            {
                if (Random.value <= gameEvent.triggerChance)
                {
                    Debug.Log($"🎯 {gameEvent.eventName}: {gameEvent.description}");
                    eventUI.ShowEvent(gameEvent.eventName, gameEvent.description);
                    gameEvent.triggerAction?.Invoke();
                }
                else
                {
                    Debug.Log($"❌ Sự kiện {gameEvent.eventName} bị bỏ qua do không đạt tỷ lệ.");
                }
                gameEvent.triggered = true;
            }
        }
    }

    void InitializeEvents()
    {
        eventList.Add(new RTSGameEvent
        {
            eventName = "Quái vật cổ đại xuất hiện",
            triggerTime = 300f,
            triggerChance = 0.3f,
            description = "Một con quái vật khổng lồ xuất hiện tại trung tâm bản đồ!",
            triggerAction = () =>
            {
                Debug.Log("🔥 Quái vật cổ đại đã xuất hiện!");// thay logic vào 
            }
        });

        eventList.Add(new RTSGameEvent
        {
            eventName = "Trận mưa lớn",
            triggerTime = 180f,
            triggerChance = 0.5f,
            description = "Mưa lớn khiến tầm nhìn của lính giảm mạnh.",
            triggerAction = () =>
            {
                // Ví dụ gọi hiệu ứng mưa hoặc giảm tầm nhìn toàn bộ lính
                Debug.Log("🌧️ Trời bắt đầu mưa lớn!");
            }
        });

        
    }
}
