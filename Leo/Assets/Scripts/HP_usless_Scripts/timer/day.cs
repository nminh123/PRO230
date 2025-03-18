using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WorldTime
{
    [RequireComponent(typeof(TMP_Text))]
    public class day : MonoBehaviour
    {
        [SerializeField]
        private worldtime _worldTime;
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            UpdateDay(newTime);
        }

        private void UpdateDay(TimeSpan currentTime)
        {
            // Tính toán số ngày dựa trên thời gian hiện tại và độ dài một ngày
            int totalDays = (int)(currentTime.TotalMinutes / worldTimeConstant.MinutesInDay);
            int daysPassed = totalDays % 30 + 1; // Lấy phần dư và cộng thêm 1 để ngày bắt đầu từ 1 đến 30
            int monthsPassed = (totalDays / 30) % 12 + 1; // Tính toán tháng (phần dư khi chia cho 12 và cộng thêm 1)
            int yearsPassed = totalDays / 360 + 1; // Tính toán năm (chia cho 360 và cộng thêm 1)

            // Hiển thị số ngày, tháng, năm lên TextMesh Pro Text
            _text.SetText($"Day {daysPassed} ");// /{monthsPassed}/ {yearsPassed} ");

        }
    }
}