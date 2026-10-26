using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// コマンドに必要な機能を搭載します
/// </summary>
public interface ICommandable
{
    /// @brief コマンド実行関数
    void Execute(GameObject user, List<GameObject> target);
}
