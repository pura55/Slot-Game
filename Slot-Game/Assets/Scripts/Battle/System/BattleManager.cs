using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/// <summary>
/// バトルマネージャー
/// 
/// バトルを管理します
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region Config
    [SerializeField] private DamageTextManager damageTextManager; // ダメージテキストの管理者
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
        joinPlayer.OnDead += PlayerLeaveBattle;

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

    /// @brief プレイヤーをバトルから退場させます
    private void PlayerLeaveBattle(IBattleable battleable)
    {
        joinPlayer.OnDead -= PlayerLeaveBattle;
        damagedPlayer.OnDamaged -= CharacterShowDamage;
        joinPlayer = null;
        damagedPlayer = null;
    }

    /// @brief 敵をバトルから退場させます
    private void EnemyLeaveBattle(IBattleable battleable)
    {
        // バトルに参加している敵の中から該当する敵を退場させます
        foreach(IBattleable join in joinEnemies)
        {
            if(join != battleable)
            {
                continue;
            }

            join.OnDead -= EnemyLeaveBattle;
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
            PlayerManager player = joinPlayer.GameObject.GetComponent<PlayerManager>();
            player.FinishBattle();
        }
    }

    /// @brief キャラクターのダメージを表示します
    private void CharacterShowDamage(Vector3 position, int damage, DamagedTarget target)
    {
        damageTextManager.ShowDamageText(position, damage, target);
    }
}
