using UnityEngine;

/// <summary>
/// ダメージテキストマネージャー
/// 
/// ダメージテキストを管理します
/// </summary>
public class DamageTextManager : MonoBehaviour
{
    #region Config
    [SerializeField] private DamageText damageText; // ダメージテキスト
    [SerializeField] private uint damagePoolSize = 10; // ダメージ用プールサイズ

    [SerializeField] private HealText healText; // 回復テキスト
    [SerializeField] private uint healPoolSize = 3; // 回復用プールサイズ
    #endregion

    #region State
    private ObjectPool<DamageText> damageTextPool; // ダメージテキストのプール
    private ObjectPool<HealText> healTextPool; // 回復テキストのプール
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IPooledObjectFactory damageFactory = new PooledTextFactory(damageText, this.transform);
        damageTextPool = new ObjectPool<DamageText>(damageFactory);
        damageTextPool.PoolSetUp(damagePoolSize);

        IPooledObjectFactory healFactory = new PooledTextFactory(healText, this.transform);
        healTextPool = new ObjectPool<HealText>(healFactory);
        healTextPool.PoolSetUp(healPoolSize);
    }

    /// @brief ダメージを表示する関数
    public void ShowDamageText(Vector3 position, int damage, DamagedTarget target)
    {
        // ダメージテキストをプールから取り出す
        DamageText damageText = damageTextPool.GetFromPool() as DamageText;

        switch (target)
        {
            case DamagedTarget.PLAYER:
                damageText.SetInfo(position, damage, DamagedTarget.PLAYER);
                break;
            case DamagedTarget.ENEMY:
                if(damageText == null)
                {
                    Debug.Log("ダメージテキストが入っていません");
                }
                damageText.SetInfo(position, damage, DamagedTarget.ENEMY);
                break;
        }
    }

    /// @brief 回復量を表示します
    public void ShowHealText(Vector3 position, int healAmount)
    {
        HealText healText = healTextPool.GetFromPool() as HealText;
        healText.SetInfo(position, healAmount);
    }
}
