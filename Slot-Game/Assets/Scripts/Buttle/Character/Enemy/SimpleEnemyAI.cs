using System;
using UnityEngine;

/// <summary>
/// シンプルエネミーAI
/// 
/// 敵のAI処理を行います
/// </summary>
public class SimpleEnemyAI : MonoBehaviour, IBattlable
{
    #region Config
    [SerializeField] private int waitingTurn = 3; // 待機ターン
    [SerializeField] private LeftTurnManager leftTurnManager; // 待機ターン管理
    #endregion

    #region State
    public event Action<GameObject> OnJoined;
    public event Action<GameObject> OnLeft;
    public bool InBattle { get { return inBattle; } set { inBattle = value; } }
    private bool inBattle = false;

    private int currentWaitTurn;
    #endregion

    void Start()
    {
        currentWaitTurn = waitingTurn;
        leftTurnManager.ChangeLeftTurn(currentWaitTurn);
    }

    void Update()
    {
    }

    public void JoinedInvoke()
    {
        OnJoined?.Invoke(this.gameObject);
    }

    /// @brief ターン減少処理を行います
    public void ReductTurn(int turn)
    {
        currentWaitTurn -= turn;
        leftTurnManager.ChangeLeftTurn(currentWaitTurn);
    }
}
