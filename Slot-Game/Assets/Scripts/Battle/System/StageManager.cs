using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// ステージマネージャー
/// </summary>
public class StageManager : MonoBehaviour
{
    #region Config
    [SerializeField] private List<GameObject> stages; // Stage
    [SerializeField] private BattleManager battleManager; // バトルマネージャー
    [SerializeField] private GameObject player; // プレイヤー
    [SerializeField] private Vector3 playerGeneratePosition = new Vector3(-8, 0f, 0f);
    #endregion

    #region State
    private List<GameObject> gameObjects = new();
    private int currentStageNumber = 0; // 現在のステージ番号
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneratePlayer();
        GenerateStage(currentStageNumber);
    }

    private void GeneratePlayer()
    {
        GameObject generatePlayer = Instantiate(player, playerGeneratePosition, Quaternion.identity);
        gameObjects.Add(generatePlayer);
    }

    /// @brief ステージを生成します
    /// @param stageNum ステージ番号
    private void GenerateStage(int stageNum)
    {
        GameObject stage = Instantiate(stages[stageNum], Vector3.zero, Quaternion.identity);
        WaveGenerator generator = stage.GetComponent<WaveGenerator>();
        if (generator != null)
        {
            generator.Battle = battleManager;
        }
        gameObjects.Add(stage);
    }

    /// @brief オブジェクトを再読み込みします
    public void ReloadObjects()
    {
    }
}
