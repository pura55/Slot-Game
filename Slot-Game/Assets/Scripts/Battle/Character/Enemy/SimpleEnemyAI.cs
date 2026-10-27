using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シンプルエネミーAI
/// 
/// 敵のAI処理を行います
/// </summary>
public class SimpleEnemyAI : MonoBehaviour, IBattleable
{
    #region Config
    [SerializeField] private int waitingTurn = 3; // 待機ターン
    [SerializeField] private LeftTurnManager leftTurnManager; // 待機ターン管理
    [SerializeField] private EnemyCommander enemyCommander; // 敵のコマンド指示
    #endregion

    #region State
    public GameObject GameObject { get { return gameObject; }} // このオブジェクトの取得用ゲッター
    public List<IBattleable> EffectTargets { get; set; } // 行動によって効果を受ける対象

    public event Action<IBattleable> OnDead; // 死亡イベント
    public event Action<IBattleable> OnAct; // 行動時のイベント

    private int currentWaitTurn; // 現在の待機ターン
    private const int activeTurn = 0; // 行動するターン
    #endregion

    void Start()
    {
        EffectTargets = new List<IBattleable>();
    }

    void Update()
    {
    }

    /// @brief バトル開始処理を行います
    public void BattleStart()
    {
        currentWaitTurn = waitingTurn;
        leftTurnManager.ChangeLeftTurn(currentWaitTurn);
    }

    /// @brief 行動実行処理を行います
    public void ExecuteAction()
    {
        OnAct?.Invoke(this);
    }

    /// @brief ターン減少処理を行います
    public void ReduceTurn(int turn)
    {
        currentWaitTurn -= turn;
        leftTurnManager.ChangeLeftTurn(currentWaitTurn);

        if(currentWaitTurn == activeTurn)
        {
            enemyCommander.OperateCommand(EnemyCommandType.LIGHT_ATTACK);
            currentWaitTurn = waitingTurn;
            leftTurnManager.ChangeLeftTurn(currentWaitTurn);
        }
    }

    /// @brief 死亡時の処理を実行します
    public void ExecuteDead()
    {
        OnDead?.Invoke(this);
        Destroy(gameObject);
    }
}
