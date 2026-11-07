using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Probability_Scriptable")]
public class SlotProbabilityDataSo : ScriptableObject
{
    // 確率のデータをリスト化する変数
    public List<SlotProbabilityDataBase> SlotProbabilities;

    /// @brief データをコピーする関数
    public void CopyData(SlotProbabilityData data)
    {
        // リストを初期化
        SlotProbabilities = new List<SlotProbabilityDataBase>();

        Debug.Log($"コピーまでは来てます");

        foreach (var SlotProbability in data.SlotProbabilities)
        {
            Debug.Log($"データコピー中");

            // 実体を作る
            SlotProbabilityDataBase progressSlotProbability = new SlotProbabilityDataBase();
            progressSlotProbability.symbolname = SlotProbability.symbolname;
            progressSlotProbability.probability = SlotProbability.probability;

            // 実体を入れる
            SlotProbabilities.Add(progressSlotProbability);
        }
    }
}
