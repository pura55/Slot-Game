using UnityEngine;

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
        IBattlable battlable = enemy.GetComponent<IBattlable>();
        battleManager.SetJoinTarget(battlable);
        SimpleEnemyAI ai = enemy.GetComponent<SimpleEnemyAI>();
        ai.JoinedInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが入りました。");
            PlayerManager manager = collider.gameObject.GetComponent<PlayerManager>();
            manager.ReadyBattle();
        }
    }
}
