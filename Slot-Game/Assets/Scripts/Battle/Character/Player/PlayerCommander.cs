using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーコマンダー
/// 
/// プレイヤーのコマンドを指示します
/// </summary>
public class PlayerCommander : MonoBehaviour
{
    #region Config
    [SerializeField] private List<PlayerCommand> playerCommands;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private PlayerManager playerManager;
    #endregion

    private void Update()
    {
        if(playerManager.State == PlayerState.BATTLE)
        {
            if(Keyboard.current.rKey.wasPressedThisFrame)
            {
                OperateCommand(CommandType.LIGHT_ATTACK);
            }
        }
    }

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
        foreach (IBattleable joinTarget in battleManager.JoinEnemies)
        {
           enemies.Add(joinTarget.GameObject);
        }

        return enemies;
    }
}