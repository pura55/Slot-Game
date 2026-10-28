using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// バトルマネージャー
/// 
/// バトルを管理します
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region Config
    [SerializeField] private DamageTextManager damageTextManager; // ダメージテキストの管理者
    [SerializeField] private GameObject DeadText; // 死亡時のテキスト
    [SerializeField] private StageManager stageManager; // ステージマネージャー
    private float reloadTime = 3f;
    #endregion

    #region State
    // キャラクター //
    private IBattleable joinPlayer; // 参加プレイヤー
    public IBattleable JoinPlayer { get { return joinPlayer; }}
    private List<IBattleable> joinEnemies = new(); // 参加している敵
    public List<IBattleable> JoinEnemies { get { return joinEnemies; }}

    private IDamageable damagedPlayer; // ダメージを受けるプレイヤー
    private List<IDamageable> damagedEnemies = new(); // ダメージを受ける敵

    private BattleState battleState = BattleState.IDLE; // バトルの状態
    #endregion

    /// @brief ウェーブバトルを開始します
    public void StartWaveBattle(List<GameObject> enemies, GameObject player, Vector3 generatePosition)
    {
        // 参加者のクリア
        joinPlayer = null;
        damagedPlayer = null;
        joinEnemies.Clear();
        damagedEnemies.Clear();

        // プレイヤー参加者登録
        joinPlayer = player.GetComponent<IBattleable>();
        joinPlayer.OnDead += PlayerDead;
        joinPlayer.OnAct += PlayerAction;

        // プレイヤーダメージ表示の登録
        damagedPlayer = player.GetComponent<IDamageable>();
        damagedPlayer.OnDamaged += CharacterShowDamage;

        // 敵の参加者・ダメージ表示登録
        foreach (GameObject enemy in enemies)
        {
            GameObject enemyObj = Instantiate(enemy, generatePosition,Quaternion.identity);
            IBattleable battleable = enemyObj.GetComponent<IBattleable>();
            joinEnemies.Add(battleable);
            battleable.OnDead += EnemyLeaveBattle;
            battleable.OnAct += EnemyAction;

            IDamageable damageable = enemyObj.GetComponent<IDamageable>();
            damagedEnemies.Add(damageable);
            damageable.OnDamaged += CharacterShowDamage;
        }

        if (joinEnemies is { Count: 0 })
        {
            Debug.LogError("敵の参照が取得できないか、生成されていない可能性があります");
            return;
        }

        // 開始処理
        joinPlayer.BattleStart();

        foreach (IBattleable enemy in joinEnemies)
        {
            enemy.BattleStart();
        }

        SetState(BattleStateInput.START);
    }

    /// @brief 状態設定を行います
    public void SetState(BattleStateInput input)
    {
        battleState = (battleState, input) switch
        {
            (BattleState.IDLE, BattleStateInput.START) => BattleState.RUNNING,
            (BattleState.RUNNING, BattleStateInput.END) => BattleState.FINISHING,
            _ => battleState
        };

        OnEnterState(battleState);
    }

    /// @brief 各状態に入った際の処理を行います
    private void OnEnterState(BattleState state)
    {
        switch(state)
        {
            case BattleState.IDLE:
                break;
            case BattleState.RUNNING:
                break;
            case BattleState.FINISHING:
                break;
        }
    }

    /// @brief プレイヤー死亡時の処理を行います
    private void PlayerDead(IBattleable battleable)
    {
        DeadText.SetActive(true);
        PlayerLeaveBattle();
        StartCoroutine(WaitRestartStage());
    }

    /// @brief プレイヤーをバトルから退場させます
    private void PlayerLeaveBattle()
    {
        Debug.Log("プレイヤーの参加登録を解除します");
        // イベントの解除
        joinPlayer.OnAct -= PlayerAction;
        joinPlayer.OnDead -= PlayerDead;
        damagedPlayer.OnDamaged -= CharacterShowDamage;


        // 敵の参照を削除
        joinPlayer.EffectTargets.Clear();

        joinPlayer = null;
        damagedPlayer = null;

    }

    /// @brief 敵をバトルから退場させます
    private void EnemyLeaveBattle(IBattleable battleable)
    {
        Debug.Log("敵の参加登録を解除します");
        // バトルに参加している敵の中から該当する敵を退場させます
        foreach (IBattleable join in joinEnemies)
        {
            if(join != battleable)
            {
                continue;
            }

            join.OnDead -= EnemyLeaveBattle;
            join.OnAct -= EnemyAction;

            joinEnemies.Remove(join);
            break;
        }

        // ダメージ表示登録を解除します
        foreach(IDamageable enemy in damagedEnemies)
        {
            if(enemy != battleable.GameObject.GetComponent<IDamageable>())
            {
                continue;
            }

            enemy.OnDamaged -= CharacterShowDamage;
            damagedEnemies.Remove(enemy);
            break;
        }
   

        // 敵が全て倒された場合、勝利処理を実行
        if(joinEnemies is { Count: 0})
        {
            if(joinPlayer ==  null)
            {
                return;
            }

            PlayerManager player = joinPlayer.GameObject.GetComponent<PlayerManager>();
            player.FinishBattle();

            // プレイヤーの登録を解除
            PlayerLeaveBattle();
        }
    }

    /// @brief キャラクターのダメージを表示します
    private void CharacterShowDamage(Vector3 position, int damage, DamagedTarget target)
    {
        damageTextManager.ShowDamageText(position, damage, target);
    }

    /// @brief プレイヤー行動時の処理を行います
    private void PlayerAction(IBattleable player)
    {
        // ターゲット数が変化していない場合
        if(player.EffectTargets.Count == JoinEnemies.Count)
        {
            return;
        }

        player.EffectTargets.Clear();
        player.EffectTargets = new List<IBattleable> (JoinEnemies);
    }

    /// @brief 敵行動時の処理を行います
    private void EnemyAction(IBattleable enemy)
    {
        // プレイヤーが既に入っている場合処理を行わない
        if(0 < enemy.EffectTargets.Count)
        {
            return;
        }

        enemy.EffectTargets.Clear();
        enemy.EffectTargets.Add(joinPlayer);
    }

    /// @brief ステージの再スタート待機
    IEnumerator WaitRestartStage()
    {
        yield return new WaitForSeconds(reloadTime);

        if (JoinEnemies.Count < 1)
        {
            Debug.Log("敵が存在しません");
        }
        
        foreach(IBattleable enemy in JoinEnemies)
        {
            enemy.ExecuteDead();
        }

        Debug.Log("ステージを再スタートします");
    }
}
