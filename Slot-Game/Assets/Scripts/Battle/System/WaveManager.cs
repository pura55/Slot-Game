using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ウェーブ管理を行います
/// </summary>
public class WaveManager : MonoBehaviour
{
    #region Config
    [Header("ENEMY")]
    [SerializeField] private List<GameObject> waveEnemies; // ウェーブの敵

    [SerializeField] private Vector3 enemyGeneratePosition = new Vector3(2f, 0f, 0f); // 生成座標
    #endregion

    #region State
    private BattleManager battleManager; // バトルマネージャー
    public BattleManager Buttle { set { battleManager = value; } }
    #endregion

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが入りました。");
            Vector3 position = transform.position + enemyGeneratePosition;
            battleManager.StartWaveBattle(waveEnemies, collider.gameObject, position);
        }
    }
}
