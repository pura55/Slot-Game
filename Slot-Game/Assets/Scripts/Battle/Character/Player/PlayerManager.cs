using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーマネージャー
/// 
/// プレイヤーの基本動作を管理します
/// </summary>
public class PlayerManager : MonoBehaviour, IBattleable
{
    #region Config
    [SerializeField] private PlayerMovement playerMovement; // プレイヤーの移動
    #endregion

    #region State
    public GameObject GameObject { get { return gameObject; } } // このオブジェクトの取得用ゲッター
    public List<IBattleable> EffectTargets { get; set; } // 行動によって効果を受ける対象

    public event Action<IBattleable> OnDead; // 死亡時のイベント
    public event Action<IBattleable> OnAct; // 行動時のイベント

    // プレイヤー状態
    private PlayerState playerState = PlayerState.IDLE; 
    public PlayerState State { get { return playerState; } private set { playerState = value; } }

    private Vector3 firstPosition = new Vector3();
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EffectTargets = new List<IBattleable>();
        firstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InputMovement();
    }

    /// @brief 入力処理を行います
    private void InputMovement()
    {
        if (Keyboard.current.dKey.isPressed && playerState == PlayerState.IDLE)
        {
            playerState = PlayerState.MOVE;
            playerMovement.SetSpeed(playerState);
        }
    }

    /// @brief バトル開始処理を行います
    public void BattleStart()
    {
        playerState = PlayerState.BATTLE;
        playerMovement.SetSpeed(playerState);
    }

    /// @brief バトル終了の処理を行います 
    public void FinishBattle()
    {
        playerState = PlayerState.IDLE;
        playerMovement.SetSpeed(playerState);
    }

    /// @brief 各状態に入った際の処理を行います
    private void OnEnterState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.MOVE:
                break;
            case PlayerState.BATTLE:
                break;
            case PlayerState.DEAD:
                break;
        }
    }

    /// @brief 行動実行処理を行います
    public void ExecuteAction()
    {
        OnAct?.Invoke(this);
    }

    /// @brief 死亡時の処理を実行します
    public void ExecuteDead()
    {
        playerState = PlayerState.DEAD;
        OnDead?.Invoke(this);
    }

    public void ObjectReset()
    {
        transform.position = firstPosition;
        playerState = PlayerState.IDLE;
        playerMovement.SetSpeed(playerState);
    }
}
