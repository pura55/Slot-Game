using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WhiteLineAnimation : MonoBehaviour
{
    [Header("ëŒè€âÊëú")]
    [SerializeField] private Image characterLine;

    [Header("èoåªéûä‘")]
    [SerializeField] private float appearTime = 0.3f;

    [Header("ägëÂó¶")]
    [SerializeField] private float startScale = 1.2f;

    [Header("î≠åıê›íË")]
    [SerializeField] private float startGlow = 8f;
    [SerializeField] private float normalGlow = 3f;

    [Header("óhÇÍê›íË")]
    [SerializeField] private float shakeTime = 0.5f;
    [SerializeField] private float shakePowerX = 20f;
    [SerializeField] private float shakePowerY = 10f;
    [SerializeField] private float shakeSpeed = 40f;

    private Material material;
    private Vector3 baseScale;
    private Vector2 basePosition;

    private Coroutine animationCoroutine;

    void Awake()
    {
        material = new Material(characterLine.material);
        characterLine.material = material;

        baseScale = characterLine.rectTransform.localScale;
        basePosition = characterLine.rectTransform.anchoredPosition;
    }

    void OnEnable()
    {
        PlayEffect();
    }

    public void PlayEffect()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        float elapsed = 0f;

        float duration = Mathf.Max(appearTime, shakeTime);

        material.SetFloat("_Opacity", 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // èoåªÉAÉjÉÅÅ[ÉVÉáÉì
            float t = Mathf.Clamp01(
                elapsed / Mathf.Max(appearTime, 0.001f)
            );

            float scale = Mathf.Lerp(startScale, 1f, t);

            characterLine.rectTransform.localScale =
                baseScale * scale;

            material.SetFloat("_Opacity", t);

            material.SetFloat(
                "_GlowStrength",
                Mathf.Lerp(startGlow, normalGlow, t)
            );

            // óhÇÍÇÃå∏êä
            float shakeT = Mathf.Clamp01(
                elapsed / Mathf.Max(shakeTime, 0.001f)
            );

            float power = 1f - shakeT;

            // â°ï˚å¸ÇÃóhÇÍ
            float x =
                Mathf.Sin(elapsed * shakeSpeed * Mathf.PI * 2f)
                * shakePowerX * power;

            // ècï˚å¸ÇÃóhÇÍ
            float y =
                Mathf.Cos(elapsed * shakeSpeed * Mathf.PI * 2f * 0.7f)
                * shakePowerY * power;

            characterLine.rectTransform.anchoredPosition =
                basePosition + new Vector2(x, y);

            yield return null;
        }

        // å≥ÇÃà íuÇ∆ÉTÉCÉYÇ…ñﬂÇ∑
        characterLine.rectTransform.anchoredPosition =
            basePosition;

        characterLine.rectTransform.localScale =
            baseScale;

        material.SetFloat("_Opacity", 1f);
        material.SetFloat("_GlowStrength", normalGlow);

        animationCoroutine = null;
    }

    void OnDisable()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        characterLine.rectTransform.anchoredPosition =
            basePosition;

        characterLine.rectTransform.localScale =
            baseScale;
    }

    void OnDestroy()
    {
        if (material != null)
        {
            Destroy(material);
        }
    }
}