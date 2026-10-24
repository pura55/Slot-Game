using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private SimpleEnemyAI simpleEnemyAI;

    #region State
    private int hitPoint = 20; // 体力
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// @brief ダメージ処理を行います
    public void Damaged(int amount)
    {
        hitPoint -= amount;

        Debug.Log("ダメージ(敵）：" + amount);

        // 0未満にならないよう固定
        if (hitPoint < 0)
        {
            hitPoint = 0;
        }

        if(hitPoint == 0)
        {
            Debug.Log("体力がありません");
            //simpleEnemyAI.
        }
    }

    /// @brief 回復処理を行います
    public void Healed(int amount)
    {
        hitPoint += amount;
    }
}
