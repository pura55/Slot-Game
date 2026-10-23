using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ効果を与えるコマンド
/// </summary>
[CreateAssetMenu(menuName = "Command_DamageEffect_Create")]
public class DamageEffect : CommandEffect
{
    [SerializeField] private int damageAmount = 10;

    public override void ApplyEffect(GameObject user, List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            damageable.Damaged(damageAmount);
        }
    }
}
