using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ステージマネージャー
/// </summary>
public class StageManager : MonoBehaviour
{
    #region Config
    [SerializeField] private List<GameObject> stages; // Stage
    [SerializeField] private BattleManager battleManager; // バトルマネージャー
    #endregion

    #region State
    private int currentStageNumber = 0; // 現在のステージ番号
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateState(currentStageNumber);
    }

    /// @brief ステージを生成します
    /// @param stageNum ステージ番号
    private void GenerateState(int stageNum)
    {
        GameObject stage = Instantiate(stages[stageNum], Vector3.zero, Quaternion.identity);
        WaveGenerater generater = stage.GetComponent<WaveGenerater>();
        if (generater != null)
        {
            generater.Battle = battleManager;
        }
    }
}
