using UnityEngine;

/// <summary>
/// プールテキストファクトリー
/// 
/// プールされるテキストを生成します
/// </summary>
public class PooledTextFactory : IPooledObjectFactory
{
    #region State
    private readonly PooledText pooledText; // テキスト
    private readonly Transform parent; // キャンバス
    #endregion

    /// @brief ファクトリーを生成する関数
    public PooledTextFactory(PooledText pooledText, Transform parentTransform)
    {
        this.pooledText = pooledText;
        this.parent = parentTransform;
    }

    public APooledObject Create()
    {
        if (pooledText == null)
        {
            Debug.LogError("プレハブが設定されていません。");
            return null;
        }

        PooledText instance = Object.Instantiate(pooledText, parent);
        return instance;
    }
}
