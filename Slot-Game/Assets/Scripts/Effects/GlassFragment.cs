using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GlassFragment : MaskableGraphic
{
    private List<Vector2> vertices = new List<Vector2>();

    private Vector2 velocity;
    private float rotationSpeed;
    private float gravity = 800f;

    private float lifeTime = 1.5f;
    private float elapsedTime;

    private bool isBreaking;
    private Color originalColor;

    [Header("輪郭の設定")]
    [SerializeField] private float edgeWidth = 2f;
    [SerializeField] private float glowWidth = 8f;

    public void Initialize(
        List<Vector2> points,
        Color glassColor)
    {
        vertices = new List<Vector2>(points);

        color = glassColor;
        originalColor = glassColor;

        raycastTarget = false;

        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (vertices.Count < 3)
            return;

        // ガラス本体
        foreach (Vector2 point in vertices)
        {
            vh.AddVert(point, color, Vector2.zero);
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            vh.AddTriangle(0, i, i + 1);
        }

        float alpha = originalColor.a > 0f
            ? color.a / originalColor.a
            : 0f;

        // ガラスの輪郭
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 a = vertices[i];

            Vector2 b =
                vertices[(i + 1) % vertices.Count];

            // 外側の青い光
            AddLine(vh, a, b, glowWidth * 2f,
                new Color(0.1f, 0.4f, 1f, alpha * 0.12f));

            // 内側の青い光
            AddLine(vh, a, b, glowWidth,
                new Color(0.2f, 0.6f, 1f, alpha * 0.35f));

            // 水色の輪郭
            AddLine(vh, a, b, edgeWidth * 2f,
                new Color(0.5f, 0.85f, 1f, alpha * 0.8f));

            // 中心の白い輪郭
            AddLine(vh, a, b, edgeWidth,
                new Color(1f, 1f, 1f, alpha));
        }
    }

    private void AddLine(
        VertexHelper vh,
        Vector2 a,
        Vector2 b,
        float width,
        Color lineColor)
    {
        Vector2 direction = (b - a).normalized;

        Vector2 normal = new Vector2(
            -direction.y,
            direction.x) * width * 0.5f;

        int index = vh.currentVertCount;

        vh.AddVert(a - normal, lineColor, Vector2.zero);
        vh.AddVert(a + normal, lineColor, Vector2.zero);
        vh.AddVert(b + normal, lineColor, Vector2.zero);
        vh.AddVert(b - normal, lineColor, Vector2.zero);

        vh.AddTriangle(index, index + 1, index + 2);
        vh.AddTriangle(index, index + 2, index + 3);
    }

    public void Break(Vector2 direction)
    {
        velocity = direction.normalized
            * Random.Range(400f, 1000f);

        rotationSpeed = Random.Range(-180f, 180f);

        elapsedTime = 0f;
        isBreaking = true;
    }

    void Update()
    {
        if (!isBreaking)
            return;

        float dt = Time.deltaTime;

        elapsedTime += dt;

        // 重力
        velocity.y -= gravity * dt;

        // 移動
        rectTransform.anchoredPosition += velocity * dt;

        // 回転
        rectTransform.Rotate(
            0f, 0f, rotationSpeed * dt);

        // フェードアウト
        Color c = originalColor;

        c.a *= 1f - Mathf.Clamp01(
            elapsedTime / lifeTime);

        color = c;

        if (elapsedTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}