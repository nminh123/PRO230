using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace WorldTime
{
    public class Bed : MonoBehaviour
    {
        [SerializeField] private worldtime _worldTime;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private GameObject panel;
        private TimeSpan _currentTime;
        public bool playerIsClose = false;

        private void Awake()
        {
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            _currentTime = newTime;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && !playerIsClose)
            {
                int currentHours = _currentTime.Hours;
                int currentMinutes = _currentTime.Minutes;

                // Kiểm tra giờ hiển thị từ 22:00 đến 04:00
                if (currentHours >= 22 || currentHours < 4)
                {
                    StartCoroutine(Sleep());
                }
                else
                {
                    StartCoroutine(Aduma());
                }
            }
        }

        private IEnumerator Aduma()
        {
            panel.SetActive(true);
            _text.SetText("You can only sleep between 22:00 and 04:00.");
            yield return new WaitForSeconds(2f);
            panel.SetActive(false);
        }

        private IEnumerator Sleep()
        {
            yield return new WaitForSeconds(0.3f);
            SetTimeToMorning();
        }

        private void SetTimeToMorning()
        {
            int currentHours = _currentTime.Hours;
            int currentMinutes = _currentTime.Minutes;

            int minutesToAdd;
            if (currentHours < 6)
            {
                minutesToAdd = (6 - currentHours) * 60 - currentMinutes;
            }
            else
            {
                minutesToAdd = (24 - currentHours + 6) * 60 - currentMinutes;
            }

            TimeSpan morningTime = _currentTime.Add(TimeSpan.FromMinutes(minutesToAdd));
            _worldTime.SetTime(morningTime);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsClose = true;
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsClose = false;
            }
        }
    }
}
