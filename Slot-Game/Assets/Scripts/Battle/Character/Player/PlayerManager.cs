using System;
using System.Collections;
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
    [SerializeField] private PlayerHealth playerHealth; // プレイヤーの体力
    [SerializeField] private PlayerAnimation playerAnimation; // プレイヤーのアニメーション
    [SerializeField] private GameObject spawnVFX; // スポーンエフェクト
    [SerializeField] private float spawnPresentationTime = 2f; // スポーンの演出時間
    #endregion

    #region State
    public GameObject GameObject { get { return gameObject; } } // このオブジェクトの取得用ゲッター
    public List<IBattleable> EffectTargets { get; set; } // 行動によって効果を受ける対象

    public event Action<IBattleable> OnDead; // 死亡時のイベント
    public event Action<IBattleable> OnAct; // 行動時のイベント

    // プレイヤー状態
    private PlayerState playerState = PlayerState.SPAWN;
    public PlayerState State { get { return playerState; } private set { playerState = value; } }

    private StageMovement stageMovement; // プレイヤーの移動（ステージの移動であるが実質的なプレイヤーの移動）
    public StageMovement Movement{ set {stageMovement = value;}}
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EffectTargets = new List<IBattleable>();
        StartCoroutine(WaitSpawnPresentation());
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
            SetState(PlayerStateInput.MOVE);
        }
    }

    /// @brief バトル開始処理を行います
    public void BattleStart()
    {
        SetState(PlayerStateInput.BATTLE);
    }

    /// @brief バトル終了の処理を行います 
    public void FinishBattle()
    {
        SetState(PlayerStateInput.LEAVE);
    }

    /// @brief 状態設定を行います
    public void SetState(PlayerStateInput input)
    {
        playerState = (playerState, input) switch
        {
            (PlayerState.SPAWN, PlayerStateInput.IDLE) => PlayerState.IDLE,
            (PlayerState.IDLE, PlayerStateInput.MOVE) => PlayerState.MOVE,
            (PlayerState.IDLE, PlayerStateInput.BATTLE) => PlayerState.BATTLE,
            (PlayerState.MOVE, PlayerStateInput.BATTLE) => PlayerState.BATTLE,
            (PlayerState.BATTLE, PlayerStateInput.LEAVE) => PlayerState.IDLE,
            (PlayerState.BATTLE, PlayerStateInput.DEAD) => PlayerState.DEAD,
            (PlayerState.DEAD, PlayerStateInput.SPAWN) => PlayerState.SPAWN,
            _ => playerState
        };

        OnEnterState(playerState);
    }

    /// @brief 各状態に入った際の処理を行います
    private void OnEnterState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.SPAWN:
                break;
            case PlayerState.IDLE:
                break;
            case PlayerState.MOVE:
                break;
            case PlayerState.BATTLE:
                break;
            case PlayerState.DEAD:
                break;
        }

        stageMovement.SetSpeed(state);
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
        stageMovement.SetFirstPosition();
        playerHealth.ResetHealth();
        playerState = PlayerState.SPAWN;
        stageMovement.SetSpeed(playerState);
    }

    /// @brief ステージの再スタート待機
    IEnumerator WaitSpawnPresentation()
    {
        GenerateSpawnVFX();

        yield return new WaitForSeconds(spawnPresentationTime);

        playerAnimation.SetSpawnPresentation();

        SetState(PlayerStateInput.IDLE);
    }

    /// @brief スポーンエフェクトを生成します
    private void GenerateSpawnVFX()
    {
        Instantiate(spawnVFX, transform.position + spawnVFX.transform.position, spawnVFX.transform.rotation);
    }
}
