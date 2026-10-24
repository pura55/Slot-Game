using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerManager : MonoBehaviour, IBattleable
{
    #region Config
    [SerializeField] private PlayerMovement playerMovement;
    #endregion

    #region State
    public event Action<GameObject> OnJoined;
    public event Action<GameObject> OnLeft;
    private PlayerState playerState = PlayerState.IDLE;
    public PlayerState State { get { return playerState; } private set { playerState = value; } }

    public bool InBattle { get { return inBattle; } set { inBattle = value; } }
    private bool inBattle = false;
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

    /// @brief 準備処理を行います
    public void ReadyBattle()
    {
        playerState = PlayerState.BATTLE;
        playerMovement.SetSpeed(playerState);
        OnJoined?.Invoke(this.gameObject);
    }

    /// @brief バトル終了の処理を行います 
    public void FinishBattle()
    {
        playerState = PlayerState.IDLE;
        playerMovement.SetSpeed(playerState);
        OnLeft?.Invoke(this.gameObject);
    }
}
