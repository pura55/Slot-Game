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
    [Header("PlayerComponents")]
    [SerializeField] private PlayerHealth playerHealth; // プレイヤーの体力
    [SerializeField] private PlayerAnimation playerAnimation; // プレイヤーのアニメーション
    [SerializeField] private PlayerCommander playerCommander; // プレイヤーコマンダー

    [Header("VFX")]
    [SerializeField] private GameObject spawnVFX; // スポーンエフェクト
    [SerializeField] private GameObject levelUpVFX; // レベルアップエフェクト

    [Header("SpawnTime")]
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

    private void OnEnable()
    {
        /// @ToDo ここにコマンドを使用する型を記入してイベントを追加してください
        /// @user ClassType.Function += OperateCommand;
        /// </例> SlotReel.OnCommand += OperateCommand;

        // イベントの登録
        Reelcontroller.OnSpin += InputMovement;

        StopButton.OnHit += InputCommand;

        PlayerLevelManager.OnLevelUp += GenerateLevelUpVFX;
    }

    private void OnDisable()
    {
        /// @ToDo ここにコマンドを使用する型を記入してイベントを削除してください
        /// @user ClassType.Function -= OperateCommand;
        /// </例> SlotReel.OnCommand -= OperateCommand;

        // イベントの解除
        Reelcontroller.OnSpin -= InputMovement;

        StopButton.OnHit -= InputCommand;

        PlayerLevelManager.OnLevelUp -= GenerateLevelUpVFX;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EffectTargets = new List<IBattleable>();
        StartCoroutine(WaitSpawnPresentation());
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// @brief 入力処理を行います
    private void InputMovement()
    {
        if (playerState != PlayerState.IDLE)
        {
            return;
        }

        SetState(CharacterStateInput.MOVE);
    }

    /// @brief コマンド入力処理を行います
    public void InputCommand(PlayerCommandType commandType)
    {
        // バトル時以外の入力は受け付けないため処理を抜けます
        if(playerState != PlayerState.BATTLE)
        {
            return;
        }

        playerCommander.OperateCommand(commandType);
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
        OnDead?.Invoke(this);
    }

    /// @brief プレイヤーのロールバックを行います
    public void ObjectReset()
    {
        stageMovement.SetFirstPosition();
        playerHealth.ResetHealth();
        SetState(CharacterStateInput.SPAWN);
        stageMovement.SetSpeed(playerState);
        StartCoroutine(WaitSpawnPresentation());
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
    
    private void GenerateLevelUpVFX()
    {
        Instantiate(levelUpVFX, transform.position + levelUpVFX.transform.position, levelUpVFX.transform.rotation, transform);
    }
}
