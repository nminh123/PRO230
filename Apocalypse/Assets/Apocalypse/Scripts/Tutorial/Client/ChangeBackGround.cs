using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackGround : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backGrounds;
    [SerializeField] private float changeTime = 60;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 1;
    [SerializeField] private float alpha = 1;

    private int currentIndex = 0;
    private float timer;

    private void Start()
    {
        LoadBackGround();
    }

    private void Update()
    {
        if (backgroundImage == null || backGrounds.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= changeTime)
        {
            timer = 0f;
            StartCoroutine(FadeToNextBackground());
        }
    }

    private IEnumerator FadeToNextBackground()
    {
        yield return Fade(alpha, 0f);

        currentIndex = (currentIndex + 1) % backGrounds.Length;

        if (backGrounds[currentIndex] != null)
            backgroundImage.sprite = backGrounds[currentIndex];

        yield return Fade(0f, alpha);
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        Color color = backgroundImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            color.a = alpha;
            backgroundImage.color = color;
            yield return null;
        }

        color.a = to;
        backgroundImage.color = color;
    }

    private void SetAlpha(float alpha)
    {
        Color color = backgroundImage.color;
        color.a = alpha;
        backgroundImage.color = color;
    }

    private void LoadBackGround()
    {
        if (backgroundImage == null || backGrounds.Length == 0) return;
        backgroundImage.sprite = backGrounds[currentIndex];
        SetAlpha(alpha);
    }
}
