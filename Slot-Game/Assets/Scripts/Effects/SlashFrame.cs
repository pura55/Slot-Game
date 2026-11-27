using UnityEngine;
using UnityEngine.UI;

public class SlashFrame : MaskableGraphic
{
    [Header("éaåÇÇÃå`èÛ")]
    [SerializeField] private float thickness = 100f;

    [Header("éaåÇÇÃêF")]
    [SerializeField] private Color bladeColor = Color.white;

    [SerializeField]
    private Color subSlashColor =
        new Color(0.4f, 0.8f, 1f, 1f);

    [Header("ç◊Ç¢éaåÇ")]
    [SerializeField] private bool showSubSlash = true;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        // ÉÅÉCÉìÇÃéaåÇ
        AddPolygon(vh, new Vector2[]
        {
            new Vector2(0.00f, 0.15f),
            new Vector2(0.23f, 0.85f),
            new Vector2(0.17f, 0.35f),
            new Vector2(0.50f, 0.85f),
            new Vector2(0.45f, 0.50f),
            new Vector2(0.78f, 1.20f),
            new Vector2(0.72f, 0.65f),
            new Vector2(1.00f, 0.90f),
            new Vector2(0.96f, 0.15f),
            new Vector2(0.38f, -0.05f),
            new Vector2(0.43f, 0.25f),
            new Vector2(0.06f, -0.25f)

        }, bladeColor);

        if (!showSubSlash)
            return;

        // â∫ë§ÇÃç◊Ç¢éaåÇ
        AddPolygon(vh, new Vector2[]
        {
            new Vector2(0.02f, -0.45f),
            new Vector2(0.35f, 0.10f),
            new Vector2(0.55f, 0.15f),
            new Vector2(0.25f, -0.30f)

        }, subSlashColor);

        // âEè„ÇÃç◊Ç¢éaåÇ
        AddPolygon(vh, new Vector2[]
        {
            new Vector2(0.60f, 1.15f),
            new Vector2(0.85f, 1.65f),
            new Vector2(0.77f, 1.25f)

        }, subSlashColor);

        // ç∂è„ÇÃç◊Ç¢éaåÇ
        AddPolygon(vh, new Vector2[]
        {
            new Vector2(0.10f, 1.20f),
            new Vector2(0.30f, 1.55f),
            new Vector2(0.23f, 1.20f)

        }, subSlashColor);
    }

    private void AddPolygon(
        VertexHelper vh,
        Vector2[] points,
        Color polygonColor)
    {
        int startIndex = vh.currentVertCount;

        Rect rect = rectTransform.rect;

        // í∏ì_Çí«â¡
        for (int i = 0; i < points.Length; i++)
        {
            UIVertex vertex = UIVertex.simpleVert;

            vertex.position = new Vector3(
                Mathf.Lerp(
                    rect.xMin,
                    rect.xMax,
                    points[i].x),
                points[i].y * thickness,
                0f
            );

            vertex.color = polygonColor * color;

            vertex.uv0 = new Vector2(
                points[i].x,
                Mathf.Clamp01(
                    (points[i].y + 0.45f) / 2.1f)
            );

            vh.AddVert(vertex);
        }

        // âöì ÇÃÇ†ÇÈÉ|ÉäÉSÉìÇéOäpå`Ç…ï™äÑ
        var indices =
            new System.Collections.Generic.List<int>();

        for (int i = 0; i < points.Length; i++)
            indices.Add(i);

        float area = 0f;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[(i + 1) % points.Length];

            area += a.x * b.y - b.x * a.y;
        }

        if (area < 0f)
            indices.Reverse();

        int safety = points.Length * points.Length;

        while (indices.Count > 2 && safety-- > 0)
        {
            bool found = false;

            for (int i = 0; i < indices.Count; i++)
            {
                int a = indices[
                    (i - 1 + indices.Count) % indices.Count];

                int b = indices[i];

                int c = indices[
                    (i + 1) % indices.Count];

                if (Cross(
                    points[b] - points[a],
                    points[c] - points[a]) <= 0f)
                    continue;

                bool contains = false;

                foreach (int p in indices)
                {
                    if (p == a || p == b || p == c)
                        continue;

                    if (InsideTriangle(
                        points[p],
                        points[a],
                        points[b],
                        points[c]))
                    {
                        contains = true;
                        break;
                    }
                }

                if (contains)
                    continue;

                vh.AddTriangle(
                    startIndex + a,
                    startIndex + b,
                    startIndex + c);

                indices.RemoveAt(i);

                found = true;
                break;
            }

            if (!found)
                break;
        }
    }

    private float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    private bool InsideTriangle(
        Vector2 p,
        Vector2 a,
        Vector2 b,
        Vector2 c)
    {
        return
            Cross(b - a, p - a) >= 0f &&
            Cross(c - b, p - b) >= 0f &&
            Cross(a - c, p - c) >= 0f;
    }
}