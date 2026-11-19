
using UnityEngine;
using System.Collections;

public class GlassScreenShake : MonoBehaviour
{
    [Header("—h‚ç‚·‘ÎÛ")]
    [SerializeField] private RectTransform shakeTarget;

    [Header("—h‚ê‚Ìİ’è")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float strength = 20f;

    private Vector2 originalPosition;
    private Coroutine shakeCoroutine;

    public void Shake()
    {
        Debug.Log("‰æ–Ê—h‚êŠJn");

        if (shakeTarget == null)
            return;

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeTarget.anchoredPosition = originalPosition;
        }

        originalPosition = shakeTarget.anchoredPosition;

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float power = 1f - elapsed / duration;

            Vector2 offset =
                Random.insideUnitCircle * strength * power;

            shakeTarget.anchoredPosition =
                originalPosition + offset;

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        shakeTarget.anchoredPosition = originalPosition;
        shakeCoroutine = null;
    }

    private void OnDisable()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);

            if (shakeTarget != null)
                shakeTarget.anchoredPosition = originalPosition;

            shakeCoroutine = null;
        }
    }
}
