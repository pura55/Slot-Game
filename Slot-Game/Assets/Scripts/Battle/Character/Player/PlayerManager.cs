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

    // ステージの移動（ステージの移動であるが実質的なプレイヤーの移動）
    private StageMovement stageMovement;
    public StageMovement Movement { set { stageMovement = value; } }
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
            SetState(CharacterStateInput.MOVE);
        }
    }

    /// @brief 状態設定を行います
    public void SetState(CharacterStateInput input)
    {
        playerState = (playerState, input) switch
        {
            (PlayerState.SPAWN, CharacterStateInput.IDLE) => PlayerState.IDLE,
            (PlayerState.IDLE, CharacterStateInput.MOVE) => PlayerState.MOVE,
            (PlayerState.MOVE, CharacterStateInput.READY) => PlayerState.READY,
            (PlayerState.READY, CharacterStateInput.BATTLE) => PlayerState.BATTLE,
            (PlayerState.BATTLE, CharacterStateInput.LEAVE) => PlayerState.IDLE,
            (PlayerState.BATTLE, CharacterStateInput.DEAD) => PlayerState.DEAD,
            (PlayerState.DEAD, CharacterStateInput.SPAWN) => PlayerState.SPAWN,
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
                ExecuteDead();
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
    private void ExecuteDead()
    {
        playerState = PlayerState.DEAD;
        OnDead?.Invoke(this);
    }

    /// @brief プレイヤーのロールバックを行います
    public void ObjectReset()
    {
        stageMovement.SetFirstPosition();
        playerHealth.ResetHealth();
        playerState = PlayerState.SPAWN;
        stageMovement.SetSpeed(playerState);
    }

    /// @brief スポーン演出の待機処理を行います
    IEnumerator WaitSpawnPresentation()
    {
        GenerateSpawnVFX();

        yield return new WaitForSeconds(spawnPresentationTime);

        playerAnimation.SetSpawnPresentation();

        SetState(CharacterStateInput.IDLE);
    }

    /// @brief スポーンエフェクトを生成します
    private void GenerateSpawnVFX()
    {
        Instantiate(spawnVFX, transform.position + spawnVFX.transform.position, spawnVFX.transform.rotation, stageMovement.StageTransfrom);
    }
}
