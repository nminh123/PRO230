using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldTime
{
    public class worldtime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;
        [SerializeField]
        private float _dayLength;

        private TimeSpan _currentTime;
        private float _minuteLength => _dayLength / worldTimeConstant.MinutesInDay;

        private void Start()
        {
            //_currentTime = TimeSpan.FromMinutes(360);
            StartCoroutine(AddMinute());
        }
        private IEnumerator AddMinute()
        {
            _currentTime += TimeSpan.FromMinutes(1);
            WorldTimeChanged?.Invoke(this, _currentTime);
            yield return new WaitForSeconds(_minuteLength);
            StartCoroutine(AddMinute());
        }
        public void SetTime(TimeSpan newTime)
        {
            _currentTime = newTime;
            WorldTimeChanged?.Invoke(this, _currentTime);

        }
    }
}
