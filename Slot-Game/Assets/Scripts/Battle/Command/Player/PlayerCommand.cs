using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのコマンド
/// </summary>
[CreateAssetMenu(menuName = "PlayerCommand_Create")]
public class PlayerCommand : ScriptableObject, ICommandable
{
    #region 
    public PlayerCommandType playerCommandType; // コマンドの種類
    public string commandDescription = string.Empty; // コマンドの説明

    public List<CommandEffect> commandEffects; // コマンドの効果
    #endregion

    /// @brief コマンドを実行します
    public void Execute(GameObject user, List<GameObject> target)
    {
        // ターゲットの参照先が存在しない場合処理を抜けます
        if(target.Count < 1)
        {
            Debug.Log("コマンド実行対象が存在しません");
            return;
        }
        // このコマンドに付属している効果を反映します
        foreach (CommandEffect effect in commandEffects)
        {
            effect.ApplyEffect(user, target);
        }
    }
}
