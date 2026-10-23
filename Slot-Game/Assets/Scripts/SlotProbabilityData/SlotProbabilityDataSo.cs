using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "probability_Scriptable")]
public class SlotProbabilityDataSo : ScriptableObject
{
    // エネミーのデータをリスト化する変数
    public List<SlotProbabilityDataBase> slotprobabilities;

    /// @brief データをコピーする関数
    public void CopyData(SlotProbabilityData data)
    {
        // リストを初期化
        slotprobabilities = new List<SlotProbabilityDataBase>();

        Debug.Log($"コピーまでは来てます");

        foreach (var slotprobability in data.slotprobabilities)
        {
            Debug.Log($"データコピー中");

            // 実体を作る
            SlotProbabilityDataBase progressslotprobability = new SlotProbabilityDataBase();
            progressslotprobability.symbolname = slotprobability.symbolname;
            progressslotprobability.probability = slotprobability.probability;

            // 実体を入れる
            slotprobabilities.Add(progressslotprobability);
        }
    }
}
