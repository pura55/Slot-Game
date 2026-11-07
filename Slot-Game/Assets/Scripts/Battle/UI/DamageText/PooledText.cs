using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// プールテキスト
/// 
/// プールされるテキストの基底クラス
/// </summary>
public abstract class PooledText : APooledObject
{
    #region Config
    [SerializeField] protected float lifeTime = 1f; // ライフタイム
    [SerializeField] protected float speed = 10f; // 設定上の速度
    [SerializeField] protected TMP_Text textMeshPro; // ダメージのテキスト
    [SerializeField] protected RectTransform rectTransform; // トランスフォーム
    #endregion


    #region State
    protected float velocity = 3f; // 移動速度
    #endregion

    /// @brief 移動処理を行う関数
    protected abstract void Movement();

    /// @brief リリースを待機する関数
    protected IEnumerator WaitToRelease()
    {
        // リリース待機
        yield return new WaitForSeconds(lifeTime);

        Release();
    }
}
