using System;
using UnityEngine;

/// <summary>
/// プールドオブジェクト
/// 
/// プールされるオブジェクトの抽象クラス
/// </summary>
public abstract class APooledObject : MonoBehaviour
{
    #region State
    private Action<APooledObject> poolAction; // プールイベント 
    #endregion

    /// @brief リリースされた際の処理を行う関数
    public void Release()
    {
        if (poolAction == null) return;
        poolAction?.Invoke(this); // 解放
    }

    /// @biref プールに登録する関数
    public void SetPoolAction(Action<APooledObject> action)
    {
        poolAction += action;
    }
}

