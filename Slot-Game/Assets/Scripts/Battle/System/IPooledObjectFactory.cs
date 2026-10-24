using UnityEngine;

/// <summary>
/// プールオブジェクトのファクトリー
/// 
/// プールオブジェクトのファクトリーに以下の機能を持たせます
/// </summary>
public interface IPooledObjectFactory
{
    /// @brief インスタンスを生成する関数
    APooledObject Create();
}
