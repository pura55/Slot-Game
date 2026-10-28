using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ウェーブマネージャー
/// </summary>
public class WaveGenerater : MonoBehaviour
{
    #region Config
    [SerializeField] private List<GameObject> waves; // ウェーブ
    [SerializeField] private float waveDistance = 2f; // ウェーブ間の距離
    #endregion

    #region State
    private Vector3 generatePosition = new Vector3(0f,0f,0f);
    private BattleManager battleManager; // バトルマネージャー
    public BattleManager Battle { set { battleManager = value; } }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateWaves();
    }

    /// @brief 1ウェーブを生成します
    private void GenerateWaves()
    {
        Vector3 generateDistance = new Vector3(waveDistance, 0f, 0f);

        foreach(GameObject wave in waves)
        {
            GameObject generatedWave = Instantiate(wave, generatePosition, Quaternion.identity);
            WaveManager waveManager = generatedWave.GetComponent<WaveManager>();
            waveManager.Buttle = battleManager;
            generatePosition += generateDistance;
        }
    }
}
