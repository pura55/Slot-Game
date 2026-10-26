using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのコマンド
/// </summary>
[CreateAssetMenu(menuName = "EnemyCommand_Create")]
public class EnemyCommand : ScriptableObject, ICommandable
{
    #region Config
    public EnemyActType enemyActType; // 行動の種類
    public string commandDescription = string.Empty; // コマンドの説明

    public List<CommandEffect> commandEffects; // コマンドの効果
    #endregion

    /// @brief コマンドを実行します
    public void Execute(GameObject user, List<GameObject> target)
    {
        // このコマンドに付属している効果を反映します
        foreach (CommandEffect effect in commandEffects)
        {
            effect.ApplyEffect(user, target);
        }
    }
}
