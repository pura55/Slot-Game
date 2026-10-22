using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    #region State
    private int hitPoint = 10; // 体力
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// @brief ダメージ処理を行います
    public void Damaged(int amount)
    {
        hitPoint -= amount;

        // 0未満にならないよう固定
        if (hitPoint < 0)
        {
            hitPoint = 0;
        }
    }
}
