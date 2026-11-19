using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Playables;

public class GlassBreakController : MonoBehaviour
{
    [Header("ƒKƒ‰ƒX")]
    [SerializeField] private RawImage glassBackground;
    [SerializeField] private RectTransform glassFragments;

    [Header("aŒ‚")]
    [SerializeField] private GameObject slashEffects;
    [SerializeField] private RawImage[] slashImages;

    [Header("aŒ‚‚ÌUV Offset")]
    [SerializeField]
    private Vector2 uvOffset =
        new Vector2(-0.15f, 0f);

    [Header("Timeline")]
    [SerializeField] private PlayableDirector director;
    [SerializeField] private GlassScreenShake screenShake;

    [Header("¬‚³‚ÈƒKƒ‰ƒX‚Ì”j•Ğ")]
    [SerializeField] private SmallGlassSpawner smallGlassSpawner;

    public void PlayEffect()
    {
        director.Stop();

        ResetEffect();

        director.time = 0;
        director.Evaluate();
        director.Play();
    }

    public void ResetEffect()
    {
        // c‚Á‚Ä‚¢‚é”j•Ğ‚ğíœ
        foreach (Transform child in glassFragments)
        {
            child.gameObject.SetActive(false);
            Destroy(child.gameObject);
        }

        // ƒKƒ‰ƒX‚ğÄ•\¦
        glassBackground.enabled = true;

        // aŒ‚‚ğÄ•\¦
        slashEffects.SetActive(true);

        // aŒ‚‚Ìis“x‚ğƒŠƒZƒbƒg
        foreach (RawImage slash in slashImages)
        {
            GlassSlashController controller =
                slash.GetComponent<GlassSlashController>();

            if (controller != null)
            {
                controller.slashProgress = -0.5f;
            }

            slash.material.SetFloat("_SlashProgress", -0.5f);
        }
    }

    public void BreakGlass()
    {
        //‰æ–Ê‚Ì—h‚ê
        if (screenShake != null)
        {
            screenShake.Shake();
        }
        
        //¬‚³‚ÈƒKƒ‰ƒX‚Ì”j•Ğ
        if (smallGlassSpawner != null)
        {
            smallGlassSpawner.SpawnFragments();
        }

        if (glassBackground == null ||
            glassFragments == null)
            return;

        Rect rect = glassFragments.rect;

        // Å‰‚Í‰æ–Ê‘S‘Ì‚ğ1–‡‚ÌƒKƒ‰ƒX‚É‚·‚é
        List<List<Vector2>> polygons =
            new List<List<Vector2>>();

        polygons.Add(new List<Vector2>
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        });

        // aŒ‚‚É‰ˆ‚Á‚Ä•ªŠ„‚·‚é
        foreach (RawImage slash in slashImages)
        {
            if (slash == null)
                continue;

            Material mat = slash.material;

            float angle =
                mat.GetFloat("_SlashAngle");

            float position =
                mat.GetFloat("_SlashPosition");

            float rad = angle * Mathf.Deg2Rad;

            Vector2 normal = new Vector2(
                Mathf.Sin(rad),
                Mathf.Cos(rad));

            float distance = position - 0.5f
                + Vector2.Dot(
                    normal,
                    Vector2.one * 0.5f - uvOffset);

            List<List<Vector2>> nextPolygons =
                new List<List<Vector2>>();

            foreach (List<Vector2> polygon in polygons)
            {
                List<Vector2> a = ClipPolygon(
                    polygon, normal, distance, true);

                List<Vector2> b = ClipPolygon(
                    polygon, normal, distance, false);

                if (a.Count >= 3)
                    nextPolygons.Add(a);

                if (b.Count >= 3)
                    nextPolygons.Add(b);
            }

            polygons = nextPolygons;
        }

        // •ªŠ„‚µ‚½ƒKƒ‰ƒX‚ğ¶¬
        foreach (List<Vector2> polygon in polygons)
        {
            CreateFragment(polygon, rect);
        }

        // Œ³‚ÌƒKƒ‰ƒX‚ÆaŒ‚‚ğ”ñ•\¦
        glassBackground.enabled = false;
        slashEffects.SetActive(false);


    }

    private List<Vector2> ClipPolygon(
        List<Vector2> polygon,
        Vector2 normal,
        float distance,
        bool positive)
    {
        List<Vector2> result = new List<Vector2>();

        for (int i = 0; i < polygon.Count; i++)
        {
            Vector2 a = polygon[i];

            Vector2 b =
                polygon[(i + 1) % polygon.Count];

            float da =
                Vector2.Dot(normal, a) - distance;

            float db =
                Vector2.Dot(normal, b) - distance;

            bool insideA = positive ? da >= 0 : da <= 0;
            bool insideB = positive ? db >= 0 : db <= 0;

            if (insideA)
                result.Add(a);

            if (insideA != insideB)
            {
                float t = da / (da - db);

                result.Add(Vector2.Lerp(a, b, t));
            }
        }

        return result;
    }

    private void CreateFragment(
        List<Vector2> polygon,
        Rect rect)
    {
        List<Vector2> points = new List<Vector2>();

        Vector2 center = Vector2.zero;

        foreach (Vector2 uv in polygon)
        {
            Vector2 point = new Vector2(
                rect.xMin + uv.x * rect.width,
                rect.yMin + uv.y * rect.height);

            points.Add(point);
            center += point;
        }

        center /= points.Count;

        // ”j•Ğ‚Ì’†S‚ğŒ´“_‚É‚·‚é
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= center;
        }

        GameObject obj = new GameObject(
            "GlassFragment",
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(GlassFragment));

        obj.transform.SetParent(glassFragments, false);

        RectTransform rt =
            obj.GetComponent<RectTransform>();

        rt.anchorMin = Vector2.one * 0.5f;
        rt.anchorMax = Vector2.one * 0.5f;
        rt.pivot = Vector2.one * 0.5f;

        rt.anchoredPosition = center;

        GlassFragment fragment =
            obj.GetComponent<GlassFragment>();

        fragment.Initialize(
            points, glassBackground.color);

        fragment.Break(
            center + Random.insideUnitCircle * 100f);
    }
}