using System;
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
    #endregion

    #region State
    public GameObject GameObject { get { return gameObject; }} // このオブジェクトの取得用ゲッター
    public event Action<IBattleable> OnDead; // 死亡イベント
    private int currentWaitTurn; // 現在の待機ターン
    private const int activeTurn = 0; // 行動するターン
    #endregion

    void Start()
    {
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

    /// @brief ターン減少処理を行います
    public void ReductTurn(int turn)
    {
        currentWaitTurn -= turn;
        leftTurnManager.ChangeLeftTurn(currentWaitTurn);

        if(currentWaitTurn == activeTurn)
        {
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
