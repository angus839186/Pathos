using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;
    public Image backgroundImage;      // UI 背景圖片
    public float transitionDuration = 1f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeBackground(Sprite newSprite)
    {
        if(backgroundImage.sprite == newSprite)
        return;
        StopAllCoroutines();
        StartCoroutine(TransitionTo(newSprite));
    }

    IEnumerator TransitionTo(Sprite newSprite)
    {
        // 先淡出背景
        Color originalColor = backgroundImage.color;
        float halfDuration = transitionDuration / 2f;
        float elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            backgroundImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(originalColor.a, 0, t));
            yield return null;
        }
        
        // 換圖
        backgroundImage.sprite = newSprite;
        
        // 淡入新圖片
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / halfDuration);
            backgroundImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, originalColor.a, t));
            yield return null;
        }
        backgroundImage.color = originalColor;
    }
}
