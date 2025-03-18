using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; // Import UI

namespace WorldTime
{
    public class iconC : MonoBehaviour
    {
        [SerializeField]
        private worldtime _worldtime;

        [SerializeField]
        private List<Schedule> _schedule;

        [SerializeField]
        private Image _iconImage;

        [SerializeField]
        private AudioSource _audioSource;

        private void Start()
        {
            _worldtime.WorldTimeChanged += CheckSchedule;
        }

        private void OnDestroy()
        {
            _worldtime.WorldTimeChanged -= CheckSchedule;
        }

        private void CheckSchedule(object sender, TimeSpan newTime)
        {
            var schedule = _schedule.FirstOrDefault(s =>
                s.Hour == newTime.Hours &&
                s.Minute == newTime.Minutes);

            if (schedule != null)
            {
                if (_iconImage != null && schedule.Icon != null)
                {
                    _iconImage.sprite = schedule.Icon; 
                }

                if (_audioSource != null && schedule.Sound != null)
                {
                    _audioSource.PlayOneShot(schedule.Sound);
                }
            }
        }

        [Serializable]
        private class Schedule
        {
            public int Hour;
            public int Minute;
            public Sprite Icon;
            public AudioClip Sound;
        }
    }
}
