using System;
using UnityEngine;

/// <summary>
/// プレイヤーヘルス
/// 
/// プレイヤーの体力を管理します
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable, IHealable
{
    #region Config
    #endregion

    #region State
    private int hitPoint = 10; // 体力

    public event Action<Vector3, int, DamagedTarget> OnDamaged; // ダメージ表示用イベント
    #endregion

    /// @brief ダメージ処理を行います
    public void Damaged(int amount)
    {
        hitPoint -= amount;

        OnDamaged?.Invoke(transform.position, amount, DamagedTarget.PLAYER);

        Debug.Log("ダメージ(プレイヤー）：" + amount);

        // 0未満にならないよう固定
        if(hitPoint < 0)
        {
            hitPoint = 0;
        }
    }

    /// @brief 回復処理を行います
    public void Healed(int amount)
    { 
        hitPoint += amount;
    }
}
