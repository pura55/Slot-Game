using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのコマンド
/// </summary>
[CreateAssetMenu(menuName = "PlayerCommand_Create")]
public class PlayerCommand : ScriptableObject, ICommandable
{
    #region 
    public CommandType commandType; // コマンドの種類
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
