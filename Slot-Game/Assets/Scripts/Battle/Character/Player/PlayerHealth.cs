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
    [SerializeField] private PlayerManager playerManager; // プレイヤー管理者
    [SerializeField] private int HP = 10; // 設定上の体力
    #endregion

    #region State
    private int currentHitPoint = 10; // 現在の体力

    public event Action<Vector3, int, DamagedTarget> OnDamaged; // ダメージ表示用イベント
    #endregion

    private void Start()
    {
        ResetHealth();
    }

    /// @brief ダメージ処理を行います
    public void Damaged(int amount)
    {
        currentHitPoint -= amount;

        OnDamaged?.Invoke(transform.position, amount, DamagedTarget.PLAYER);

        Debug.Log("ダメージ(プレイヤー）：" + amount);

        // 0未満にならないよう固定
        if(currentHitPoint < 0)
        {
            currentHitPoint = 0;
        }

        if (currentHitPoint == 0)
        {
            Debug.Log("体力がありません");
            playerManager.ExecuteDead();
        }
    }

    /// @brief 回復処理を行います
    public void Healed(int amount)
    { 
        currentHitPoint += amount;
    }

    /// @brief 体力をリセットします
    public void ResetHealth()
    {
        currentHitPoint = HP;
    }
}
