
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SmallGlassSpawner : MonoBehaviour
{
    [Header("îjï–ÇÃê›íË")]
    [SerializeField] private int fragmentCount = 40;
    [SerializeField] private float minSize = 5f;
    [SerializeField] private float maxSize = 25f;

    [Header("îÚÇ—éUÇÈë¨ìx")]
    [SerializeField] private float minSpeed = 300f;
    [SerializeField] private float maxSpeed = 900f;
    [SerializeField] private float gravity = 500f;

    [Header("è¡Ç¶ÇÈÇ‹Ç≈ÇÃéûä‘")]
    [SerializeField] private float lifeTime = 1.2f;

    private Sprite glassSprite;
    private Texture2D glassTexture;

    private void Awake()
    {
        CreateGlassSprite();
    }

    // éOäpå`ÇÃÉKÉâÉXëfçﬁÇê∂ê¨
    private void CreateGlassSprite()
    {
        int size = 64;

        glassTexture = new Texture2D(
            size, size, TextureFormat.RGBA32, false
        );

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float u = x / (float)(size - 1);
                float v = y / (float)(size - 1);

                float edge = (1f - v) * 0.5f;

                bool inside =
                    u >= 0.5f - edge &&
                    u <= 0.5f + edge;

                if (!inside)
                {
                    glassTexture.SetPixel(
                        x, y, Color.clear
                    );

                    continue;
                }

                bool border =
                    Mathf.Abs(u - (0.5f - edge)) < 0.035f ||
                    Mathf.Abs(u - (0.5f + edge)) < 0.035f ||
                    v < 0.035f;

                Color color = border
                    ? new Color(1f, 1f, 1f, 1f)
                    : new Color(0.65f, 0.85f, 1f, 0.4f);

                glassTexture.SetPixel(x, y, color);
            }
        }

        glassTexture.Apply();

        glassSprite = Sprite.Create(
            glassTexture,
            new Rect(0, 0, size, size),
            new Vector2(0.5f, 0.5f)
        );
    }

    // îjï–Çê∂ê¨Ç∑ÇÈ
    public void SpawnFragments()
    {
        if (glassSprite == null)
            CreateGlassSprite();

        for (int i = 0; i < fragmentCount; i++)
        {
            GameObject fragment = new GameObject(
                "SmallGlass_" + i,
                typeof(RectTransform),
                typeof(Image)
            );

            fragment.transform.SetParent(
                transform, false
            );

            RectTransform rect =
                fragment.GetComponent<RectTransform>();

            Image image =
                fragment.GetComponent<Image>();

            image.sprite = glassSprite;
            image.raycastTarget = false;

            float size = Random.Range(
                minSize, maxSize
            );

            rect.sizeDelta = new Vector2(
                size,
                size * Random.Range(0.8f, 2f)
            );

            rect.anchoredPosition = new Vector2(
                Random.Range(-500f, 500f),
                Random.Range(-200f, 200f)
            );

            rect.localRotation = Quaternion.Euler(
                0, 0, Random.Range(0f, 360f)
            );

            Vector2 direction =
                Random.insideUnitCircle.normalized;

            Vector2 velocity =
                direction *
                Random.Range(minSpeed, maxSpeed);

            float rotationSpeed =
                Random.Range(-720f, 720f);

            StartCoroutine(
                MoveFragment(
                    rect,
                    image,
                    velocity,
                    rotationSpeed
                )
            );
        }
    }

    // îjï–ÇÃà⁄ìÆÇ∆âÒì]
    private IEnumerator MoveFragment(
        RectTransform rect,
        Image image,
        Vector2 velocity,
        float rotationSpeed)
    {
        float elapsed = 0f;

        while (elapsed < lifeTime)
        {
            float dt = Time.unscaledDeltaTime;

            velocity.y -= gravity * dt;

            rect.anchoredPosition += velocity * dt;

            rect.Rotate(
                0, 0, rotationSpeed * dt
            );

            elapsed += dt;

            float alpha = 1f -
                Mathf.Clamp01(elapsed / lifeTime);

            image.color = new Color(
                1f, 1f, 1f, alpha
            );

            yield return null;
        }

        Destroy(rect.gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        for (int i = transform.childCount - 1;
             i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void OnDestroy()
    {
        if (glassSprite != null)
            Destroy(glassSprite);

        if (glassTexture != null)
            Destroy(glassTexture);
    }
}
