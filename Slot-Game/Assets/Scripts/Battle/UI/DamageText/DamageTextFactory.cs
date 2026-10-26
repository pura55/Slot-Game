using UnityEngine;

/// <summary>
/// ダメージテキストファクトリー
/// 
/// ダメージテキストを生成します
/// </summary>
public class DamageTextFactory : IPooledObjectFactory
{
    #region State
    private readonly DamageText damageText; // ダメ―ジテキスト
    private readonly Transform parent; // ダメージキャンバス
    #endregion

    /// @brief ファクトリーを生成する関数
    public DamageTextFactory(DamageText damageText, Transform parentTransform)
    {
        this.damageText = damageText;
        this.parent = parentTransform;
    }

    public APooledObject Create()
    {
        if (damageText == null)
        {
            Debug.LogError("プレハブが設定されていません。");
            return null;
        }

        DamageText instance = Object.Instantiate(damageText, parent);
        return instance;
    }
}
