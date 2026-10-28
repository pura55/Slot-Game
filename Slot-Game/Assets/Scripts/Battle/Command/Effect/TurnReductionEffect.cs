using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ターンを減少させるコマンド効果
/// </summary>
/// 
[CreateAssetMenu(menuName = "Command_TurnReductionEffect_Create")]
public class TurnReductionEffect : CommandEffect
{
    [SerializeField] private int reductionTurn = 1; // 減少ターン

    public override void ApplyEffect(GameObject user, List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            if(target == null)
            {
                continue;
            }

            SimpleEnemyAI enemyAI = target.GetComponent<SimpleEnemyAI>();

            enemyAI.ReduceTurn(reductionTurn);
        }
    }
}
