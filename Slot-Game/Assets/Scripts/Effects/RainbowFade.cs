using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class RainbowFade : MonoBehaviour
{
    [SerializeField] private RawImage rainbowImage;

    [Header("“§–¾“x")]
    [Range(0f, 1f)]
    public float fadeAlpha = 0f;

    void Update()
    {
        if (rainbowImage == null)
            return;

        if (rainbowImage.material == null)
            return;

        rainbowImage.material.SetFloat("_FadeAlpha", fadeAlpha);
    }
}