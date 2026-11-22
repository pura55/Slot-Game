using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バーチャルエフェクトを発生させるコマンド
/// </summary>
[CreateAssetMenu(menuName = "Command_VisualEffect_Create")]
public class VisualEffect : CommandEffect
{
    [SerializeField] private GameObject generateVisualEffect; // エフェクト

    public override void ApplyEffect(GameObject user, List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            if (target == null)
            {
                continue;
            }

            Instantiate(generateVisualEffect, target.transform.position, Quaternion.identity);
        }
    }
}
