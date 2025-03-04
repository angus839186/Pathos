using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class glowController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tmpText;
    public float duration = 1f;
    private Material tmpMaterial;

    // 初始狀態（無發光）
    public float initialGlow = 0f;
    // 目標狀態（發光）
    public float targetGlow = 1f;

    void Awake()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        // 避免改動全局材質，產生獨立的實例
        tmpMaterial = Instantiate(tmpText.fontMaterial);
        tmpText.fontMaterial = tmpMaterial;

        // 設定初始狀態：灰色且無發光
        tmpMaterial.SetFloat(ShaderUtilities.ID_GlowPower, initialGlow);
    }

    // 當滑鼠移入
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(GlowTransition(targetGlow));
    }

    // 當滑鼠移出
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(GlowTransition(initialGlow));
    }

    public IEnumerator GlowTransition(float target)
    {
        float startGlow = tmpMaterial.GetFloat(ShaderUtilities.ID_GlowPower);
        Color startColor = tmpText.color;
        Color targetColor = (target == targetGlow) ? new Color(1,1,1,1) : new Color(0.5f, 0.5f, 0.5f, 0.5f);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);


            float currentGlow = Mathf.Lerp(startGlow, target, t);
            tmpMaterial.SetFloat(ShaderUtilities.ID_GlowPower, currentGlow);


            tmpText.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }


        tmpMaterial.SetFloat(ShaderUtilities.ID_GlowPower, target);
        tmpText.color = targetColor;
    }

}
