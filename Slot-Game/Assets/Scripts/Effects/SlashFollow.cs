
using UnityEngine;

public class SlashFollow : MonoBehaviour
{
    [Header("’Ç]‚·‚é‘ÎÛ")]
    [SerializeField] private Transform target;

    [Header("ˆÊ’u‚Ì’²®")]
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.position = target.position + offset;
    }
}
