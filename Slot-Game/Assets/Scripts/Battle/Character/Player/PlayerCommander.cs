using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCommander : MonoBehaviour
{
    #region Config
    [Header("UsingCommand")]
    [SerializeField] private List<PlayerCommand> playerCommands; // プレイヤーのコマンド

    [Header("PlayerComponents")]
    [SerializeField] private PlayerManager playerManager; // プレイヤー管理者
    #endregion
    private void Update()
    {
        // 敵の行動中やプレイヤーが既に行動中の場合、処理を受け付けない
        if (BattleStateObserver.EnemyBattleState != CharacterBattleState.IDLE || BattleStateObserver.PlayerBattleState != CharacterBattleState.IDLE)
        {
            return;
        }

        if(DebugManager.IsDebug)
        {
            DebugInputs();
        }
    }

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

    /// @brief デバック時のキー入力を行います 
    private void DebugInputs()
    {
        if (playerManager.State == PlayerState.BATTLE)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.LIGHT_ATTACK);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.HEAVY_ATTACK);
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.HEAL);
            }

            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                OperateCommand(PlayerCommandType.BONUS);
            }

        }
    }
}