using UnityEngine;

public class SlotProbabilityDataLoader : DataLoaderBase
{
    #region Config
    [SerializeField] private SlotProbabilityDataSo slotprobabilityDataSo;
    #endregion

    private void Awake()
    {
        LoadData();
    }

    protected override void LoadData()
    {
        // 共通のJSON読み込み機能を利用
        SlotProbabilityData data = JsonDataLoader.Load<SlotProbabilityData>(textAsset);

        // 読み込みに成功した場合
        if (data != null)
        {
            slotprobabilityDataSo.CopyData(data);

            Debug.Log("データが読み込まれました。");
        }
    }
}