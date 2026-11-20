
using System.Collections;
using UnityEngine;

public class SlashStretch : MonoBehaviour
{
    [Header("斬撃の向き")]
    [SerializeField] private float angle = 45f;

    [Header("斬撃の長さ")]
    [SerializeField] private float slashLength = 1f;

    [Header("伸びる時間")]
    [SerializeField] private float duration = 0.2f;

    [Header("開始位置の調整")]
    [SerializeField] private Vector3 startOffset = Vector3.zero;

    [Header("斬撃Meshの横幅")]
    [SerializeField] private float meshWidth = 10f;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Coroutine stretchCoroutine;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void PlayStretch()
    {
        if (stretchCoroutine != null)
            StopCoroutine(stretchCoroutine);

        gameObject.SetActive(true);
        stretchCoroutine = StartCoroutine(Stretch());
    }

    private IEnumerator Stretch()
    {
        Quaternion rotation =
            originalRotation * Quaternion.Euler(0f, 0f, angle);

        Vector3 finalScale = new Vector3(
            originalScale.x * slashLength,
            originalScale.y,
            originalScale.z
        );

        // 最終的な斬撃の中心
        Vector3 center = originalPosition + startOffset;

        // 親のローカル座標での伸びる方向
        Vector3 direction = rotation * Vector3.right;

        // Meshの最終的な横幅
        float fullWidth = meshWidth * finalScale.x;

        // 左下の端の座標
        Vector3 startPosition =
            center - direction * (fullWidth * 0.5f);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float progress = Mathf.Clamp01(
                elapsed / Mathf.Max(duration, 0.001f)
            );

            transform.localRotation = rotation;

            transform.localScale = new Vector3(
                finalScale.x * Mathf.Max(progress, 0.001f),
                finalScale.y,
                finalScale.z
            );

            // 左下を固定して右上へ伸ばす
            transform.localPosition =
                startPosition +
                direction * (fullWidth * progress * 0.5f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = rotation;
        transform.localScale = finalScale;
        transform.localPosition = center;

        stretchCoroutine = null;
    }
}
