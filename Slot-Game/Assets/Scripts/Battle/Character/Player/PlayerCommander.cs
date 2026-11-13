using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommander : MonoBehaviour
{
    #region Config
    [SerializeField] private List<PlayerCommand> playerCommands; // プレイヤーのコマンド
    [SerializeField] private PlayerManager playerManager; // プレイヤー管理者
    #endregion
    private void Update()
    {
        // 敵の行動中やプレイヤーが既に行動中の場合、処理を受け付けない
        if (BattleStateObserver.EnemyBattleState != CharacterBattleState.IDLE || BattleStateObserver.PlayerBattleState != CharacterBattleState.IDLE)
        {
            return;
        }

        if (playerManager.State == PlayerState.BATTLE)
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.LIGHT_ATTACK);
            }

            if (Keyboard.current.bKey.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.BONUS);
            }

            if (Keyboard.current.hKey.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.HEAL);
            }
        }
    }

    private void OnEnable()
    {
        /// @ToDo ここにコマンドを使用する型を記入してイベントを追加してください
        /// @user ClassType.Function += OperateCommand;
        /// </例> SlotReel.OnCommand += OperateCommand;
    }

    private void OnDisable()
    {
        /// @ToDo ここにコマンドを使用する型を記入してイベントを削除してください
        /// @user ClassType.Function -= OperateCommand;
        /// </例> SlotReel.OnCommand -= OperateCommand;
    }

    /// @brief コマンドを指令します
    /// @user 使用する場合はイベント処理として実行してください
    ///       </例> event Action<PlayerCommandType> OnCommand;
    ///             OnCommand?.Invoke(PlayerCommandType userOperationType);
    public void OperateCommand(PlayerCommandType type)
    {
        if (playerManager.State != PlayerState.BATTLE)
        {
            return;
        }

        BattleStateObserver.PlayerBattleState = CharacterBattleState.ACTION;

        playerManager.ExecuteAction();

        // 指令されたコマンドが存在する場合実行します
        foreach (PlayerCommand command in playerCommands)
        {
            if (command.playerCommandType != type)
            {
                continue;
            }

            // プレイヤーの死亡後コマンド指令される可能性を防ぎます
            if (playerManager.State == PlayerState.DEAD)
            {
                return;
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