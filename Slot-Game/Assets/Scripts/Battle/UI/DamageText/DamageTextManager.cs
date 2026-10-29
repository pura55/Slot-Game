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
    [SerializeField] private uint poolSize = 10; // プールサイズ
    #endregion

    #region State
    private ObjectPool<DamageText> textPool; // ダメージテキストのプール
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IPooledObjectFactory factory = new DamageTextFactory(damageText, this.transform);
        textPool = new ObjectPool<DamageText>(factory);
        textPool.PoolSetUp(poolSize);
    }

    /// @brief ダメージを表示する関数
    public void ShowDamageText(Vector3 position, int damage, DamagedTarget target)
    {
        // ダメージテキストをプールから取り出す
        DamageText damageText = textPool.GetFromPool() as DamageText;

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
}
