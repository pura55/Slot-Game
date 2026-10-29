using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// コマンド効果の基底クラス
/// </summary>
public abstract class CommandEffect : ScriptableObject
{
    /// @brief 効果を実行処理を行います
    public abstract void ApplyEffect(GameObject user, List<GameObject> target);
}
