using System;
using UnityEngine;

/// <summary>
/// エネミーヘルス
/// 
/// 敵の体力を管理します
/// </summary>
public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region Config
    [SerializeField] private SimpleEnemyAI simpleEnemyAI; // 敵AI
    #endregion

    #region State
    private int hitPoint = 20; // 体力

    public event Action<Vector3, int, DamagedTarget> OnDamaged; // ダメージ表示用イベント
    #endregion

    /// @brief ダメージ処理を行います
    public void Damaged(int amount)
    {
        // 体力が既に0の場合処理を抜けます
        if(hitPoint == 0)
        {
            return;
        }

        hitPoint -= amount;

        OnDamaged?.Invoke(transform.position, amount, DamagedTarget.ENEMY);

        Debug.Log("ダメージ(敵）：" + amount);

        // 0未満にならないよう固定
        if (hitPoint < 0)
        {
            hitPoint = 0;
        }

        if(hitPoint == 0)
        {
            Debug.Log("体力がありません");
            simpleEnemyAI.ExecuteDead();
        }
    }
}
