using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ‰ñ•œŒø‰Ê‚ð—^‚¦‚éƒRƒ}ƒ“ƒh
/// </summary>
[CreateAssetMenu(menuName = "Command_HealEffect_Create")]
public class HealEffect : CommandEffect
{
    public  int healAmount = 50; // ‰ñ•œ—Ê

    public override void ApplyEffect(GameObject user, List<GameObject> targets)
    {
        foreach(GameObject target in targets)
        {
            IHealable damageable = target.GetComponent<IHealable>();

            damageable.Healed(healAmount);
        }
    }
}
