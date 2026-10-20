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
    #endregion

    #region State
    public event Action<GameObject> OnJoined;
    public event Action<GameObject> OnLeft;
    public bool InBattle { get { return inBattle; } set { inBattle = value; } }
    private bool inBattle = false;
    #endregion

    void Start()
    {
    }

    void Update()
    {
    }

    public void JoinedInvoke()
    {
        OnJoined?.Invoke(this.gameObject);
    }
}
