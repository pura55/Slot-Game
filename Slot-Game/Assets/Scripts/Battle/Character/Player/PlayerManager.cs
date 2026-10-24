using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerManager : MonoBehaviour, IBattleable
{
    #region Config
    [SerializeField] private PlayerMovement playerMovement;
    #endregion

    #region State
    public GameObject GameObject { get; }
    public event Action<IBattleable> OnDead;
    private PlayerState playerState = PlayerState.IDLE;
    public PlayerState State { get { return playerState; } private set { playerState = value; } }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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

    private void ExecuteDead()
    {
        OnDead?.Invoke(this);
    }
}
