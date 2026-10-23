using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーのコマンド
/// </summary>
[CreateAssetMenu(menuName = "PlayerCommand_Create")]
public class PlayerCommand : ScriptableObject
{
    public CommandType commandName;
    public string commandDescription = string.Empty;
}
