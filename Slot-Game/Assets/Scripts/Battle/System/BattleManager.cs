using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// バトルマネージャー
/// 
/// バトルを管理します
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region State
    private IBattleable joinPlayer; // 参加プレイヤー
    public IBattleable JoinPlayer { get { return joinPlayer; } private set { joinPlayer = value; } }
    private List<IBattleable> joinEnemies = new(); // 参加している敵
    public List<IBattleable> JoinEnemies { get { return joinEnemies; } private set { joinEnemies = value; } }

    private BattleState battleState = BattleState.IDLE; // バトルの状態
    #endregion

    /// @brief ウェーブバトルを開始します
    public void StartWaveBattle(List<GameObject> enemies, GameObject player, Vector3 generatePosition)
    {
        // 参加者のクリア
        JoinPlayer = null;
        JoinEnemies.Clear();

        // 参加者登録
        JoinPlayer = player.GetComponent<IBattleable>();
        joinPlayer.OnDead += PlayerLeaveBattle;

        foreach (GameObject enemy in enemies)
        {
            GameObject enemyObj = Instantiate(enemy, generatePosition,Quaternion.identity);
            IBattleable battleable = enemy.GetComponent<IBattleable>();
            JoinEnemies.Add(battleable);
            battleable.OnDead += EnemyLeaveBattle;
        }

        // 開始処理
        joinPlayer.BattleStart();

        foreach (IBattleable enemy in joinEnemies)
        {
            enemy.BattleStart();
        }
        SetState(BattleStateInput.START);
    }

    /// @brief 状態設定を行います
    public void SetState(BattleStateInput input)
    {
        battleState = (battleState, input) switch
        {
            (BattleState.IDLE, BattleStateInput.START) => BattleState.RUNNING,
            (BattleState.RUNNING, BattleStateInput.END) => BattleState.FINISHING,
            _ => battleState
        };

        OnEnterState(battleState);
    }

    /// @brief 各状態に入った際の処理を行います
    private void OnEnterState(BattleState state)
    {
        switch(state)
        {
            case BattleState.IDLE:
                break;
            case BattleState.RUNNING:
                break;
            case BattleState.FINISHING:
                break;
        }
    }

    /// @brief プレイヤーをバトルから退場させます
    private void PlayerLeaveBattle(IBattleable battleable)
    {
        joinPlayer.OnDead -= PlayerLeaveBattle;
        joinPlayer = null;
    }

    /// @brief 敵をバトルから退場させます
    private void EnemyLeaveBattle(IBattleable battleable)
    {
        foreach(IBattleable join in joinEnemies)
        {
            if(join != battleable)
            {
                continue;
            }

            join.OnDead -= EnemyLeaveBattle;
        }
        joinEnemies.Remove(battleable);
    }
}
