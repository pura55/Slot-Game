using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーコマンダー
/// 
/// プレイヤーの指示を行います
/// </summary>
public class PlayerCommander : MonoBehaviour
{
    #region Config
    [SerializeField] private List<PlayerCommand> playerCommands;
    [SerializeField] private BattleManager battleManager; 
    #endregion

    /// @brief コマンドを指令します
    public void OperateCommand(CommandType type)
    {
        // 指令されたコマンドが存在する場合実行します
        foreach(PlayerCommand command in  playerCommands)
        {
            if(command.commandType != type)
            {
                continue;
            }

            command.Execute(gameObject, GetEnemies());
            return;
        }
    }

    /// @brief 参加している敵を取得します
    private List<GameObject> GetEnemies()
    {
        List<GameObject> enemies = new List<GameObject>();

        // 参加している敵を追加します
        foreach (GameObject joinTarget in battleManager.GetJoinCharacter())
        {
           if(!joinTarget.CompareTag("Enemy"))
            {
                continue;
            }

           enemies.Add(joinTarget);
        }

        return enemies;
    }
}