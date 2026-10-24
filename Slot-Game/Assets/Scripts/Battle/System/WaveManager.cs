using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ウェーブ管理を行います
/// </summary>
public class WaveManager : MonoBehaviour
{
    #region Config
    [SerializeField] private BattleManager battleManager; // バトルマネージャー

    [Header("ENEMY")]
    [SerializeField] private GameObject FirstEnemy;   // 敵1
    [SerializeField] private GameObject SecondEnemy;  // 敵2
    [SerializeField] private GameObject ThirdEnemy;   // 敵3
    #endregion

    #region State
    private List<GameObject> enemies = new();
    #endregion 

    void Start()
    {
        GenerateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// @brief 敵を生成します
    private void GenerateEnemy()
    {
        GameObject enemy = Instantiate(FirstEnemy, transform.position, Quaternion.identity);

        enemies.Add(enemy);

        // バトルに参加する対象の設定
        IBattleable battlable = enemy.GetComponent<IBattleable>();
        battleManager.SetJoinTarget(battlable);

        // バトルに参加
        SimpleEnemyAI ai = enemy.GetComponent<SimpleEnemyAI>();
        ai.JoinedInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが入りました。");
            battleManager.SetJoinTarget(collider.gameObject.GetComponent<IBattleable>());
            PlayerManager manager = collider.gameObject.GetComponent<PlayerManager>();
            manager.ReadyBattle();
        }
    }
}
