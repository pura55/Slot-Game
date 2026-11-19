using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class GlassSlashController : MonoBehaviour
{
    [SerializeField] private RawImage slashImage;

    [Header("éaåÇÇÃêiçsìx")]
    [Range(-0.5f, 1.5f)]
    public float slashProgress = -0.5f;

    void Update()
    {
        if (slashImage == null)
            return;

        if (slashImage.material == null)
            return;

        slashImage.material.SetFloat(
            "_SlashProgress", slashProgress);
    }
}