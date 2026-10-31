using System;
using System.Collections;
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
    [SerializeField] private EnemyMovement enemyMovement; // 敵の移動
    #endregion

    #region State
    public GameObject GameObject { get { return gameObject; } } // このオブジェクトの取得用ゲッター
    public List<IBattleable> EffectTargets { get; set; } // 行動によって効果を受ける対象

    public event Action<IBattleable> OnDead; // 死亡イベント
    public event Action<IBattleable> OnAct; // 行動時のイベント

    private EnemyState enemyState = EnemyState.SPAWN; // 敵の状態

    private int currentWaitTurn; // 現在の待機ターン
    private const int activeTurn = 0; // 行動するターン

    private const float resetTime = 0.5f; // リセット時間
    #endregion
    void Start()
    {
        EffectTargets = new List<IBattleable>();
        SetState(CharacterStateInput.MOVE);
    }

    void Update()
    {
    }

    /// @brief 状態設定を行います
    public void SetState(CharacterStateInput input)
    {
        enemyState = (enemyState, input) switch
        {
            (EnemyState.SPAWN, CharacterStateInput.MOVE) => EnemyState.MOVE,
            (EnemyState.MOVE, CharacterStateInput.BATTLE) => EnemyState.BATTLE,
            (EnemyState.BATTLE, CharacterStateInput.DEAD) => EnemyState.DEAD,
            _ => enemyState
        };

        OnEnterState(enemyState);
    }

    /// @brief 各状態に入った際の処理を行います
    private void OnEnterState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.MOVE:
                enemyMovement.OperateMovement();
                break;
            case EnemyState.BATTLE:
                currentWaitTurn = waitingTurn;
                leftTurnManager.OnText();
                leftTurnManager.ChangeLeftTurn(currentWaitTurn);
                break;
            case EnemyState.DEAD:
                ExecuteDead();
                break;
        }
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

        if (currentWaitTurn == activeTurn)
        {
            enemyCommander.OperateCommand(EnemyCommandType.LIGHT_ATTACK);
            currentWaitTurn = waitingTurn;
            leftTurnManager.ChangeLeftTurn(currentWaitTurn);
        }
    }

    /// @brief 死亡時の処理を実行します
    private void ExecuteDead()
    {
        Debug.Log("敵が死亡します");
        OnDead?.Invoke(this);
        Destroy(gameObject);
    }

    public void ObjectReset()
    {
        StartCoroutine(WaitReset());
    }

    /// @brief リセットタイム
    private IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(resetTime);

        ExecuteDead();
    }
}
