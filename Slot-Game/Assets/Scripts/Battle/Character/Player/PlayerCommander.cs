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
    [SerializeField] private List<PlayerCommand> playerCommands; // プレイヤーのコマンド
    [SerializeField] private PlayerManager playerManager; // プレイヤー管理者
    #endregion

    private void Update()
    {
        if(playerManager.State == PlayerState.BATTLE)
        {
            if(Keyboard.current.rKey.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.LIGHT_ATTACK);
            }
        }
    }

    /// @brief コマンドを指令します
    public void OperateCommand(PlayerCommandType type)
    {
        playerManager.ExecuteAction();

        // 指令されたコマンドが存在する場合実行します
        foreach (PlayerCommand command in  playerCommands)
        {
            if(command.playerCommandType != type)
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
        foreach (IBattleable joinTarget in playerManager.EffectTargets)
        {
           enemies.Add(joinTarget.GameObject);
        }

        return enemies;
    }
}