using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventNotificationUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descText;
    public CanvasGroup canvasGroup;
    public float showDuration = 3f;

    private Coroutine currentRoutine;

    public void ShowEvent(string title, string description)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(title, description));
    }

    private IEnumerator ShowRoutine(string title, string description)
    {
        // Reset UI
        titleText.text = "";
        descText.text = "";
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Hiện tiêu đề
        titleText.text = title;
        yield return new WaitForSeconds(1.5f);

        // Hiện mô tả
        descText.text = description;
        yield return new WaitForSeconds(showDuration);

        // Ẩn dần
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1 - t;
            yield return null;
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
}
