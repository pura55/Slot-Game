using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージを与えるコマンド効果
/// </summary>
[CreateAssetMenu(menuName = "Command_DamageEffect_Create")]
public class DamageEffect : CommandEffect
{
    [SerializeField] private int damageAmount = 10; // ダメージ量

    public override void ApplyEffect(GameObject user, List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            if (target == null)
            {
                continue;
            }

            IDamageable damageable = target.GetComponent<IDamageable>();

            damageable.Damaged(damageAmount);
        }
    }
}
