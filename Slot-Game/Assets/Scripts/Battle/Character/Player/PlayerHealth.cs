using UnityEngine;

/// <summary>
/// プレイヤーヘルス
/// 
/// プレイヤーの体力を管理します
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    #region Config
    #endregion

    #region State
    private int hitPoint = 10; // 体力
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// @brief ダメージ処理を行います
    public void Damaged(int amount)
    {
        hitPoint -= amount;

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
