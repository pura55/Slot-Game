using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlashOutlineGlow : BaseMeshEffect
{
    [Header("î≠åıê›íË")]
    [SerializeField]
    private Color glowColor =
        new Color(0f, 0.5f, 1f, 0.12f);

    [SerializeField] private float glowSize = 15f;

    [Range(4, 24)]
    [SerializeField] private int directions = 12;

    [Range(1, 5)]
    [SerializeField] private int layers = 3;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        List<UIVertex> original =
            new List<UIVertex>();

        vh.GetUIVertexStream(original);

        List<UIVertex> result =
            new List<UIVertex>();

        for (int layer = layers; layer >= 1; layer--)
        {
            float rate = (float)layer / layers;

            float distance = glowSize * rate;

            for (int i = 0; i < directions; i++)
            {
                float angle =
                    i * Mathf.PI * 2f / directions;

                Vector3 offset = new Vector3(
                    Mathf.Cos(angle) * distance,
                    Mathf.Sin(angle) * distance,
                    0f);

                foreach (UIVertex source in original)
                {
                    UIVertex vertex = source;

                    vertex.position += offset;

                    Color c = glowColor;

                    c.a *= (1f - rate * 0.8f);

                    c.a *= source.color.a / 255f;

                    vertex.color = c;

                    result.Add(vertex);
                }
            }
        }

        result.AddRange(original);

        vh.Clear();

        vh.AddUIVertexTriangleStream(result);
    }
}