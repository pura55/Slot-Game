using System;
using UnityEngine;

/// <summary>
/// プールドオブジェクト
/// 
/// プールされるオブジェクトの抽象クラス
/// </summary>
public abstract class APooledObject : MonoBehaviour
{
    // イベント通知型にするためにActionを採用
    private Action<APooledObject> poolAction;

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

