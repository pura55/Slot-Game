using UnityEngine;
using System.Collections.Generic;

public class SlotProbabilityDataLoader : DataLoaderBase
{
    #region Config
    [Header("スロットデータのTextAssetはこちらに入れてください")]
    [SerializeField] private List<TextAsset> textAssets;
    [SerializeField] private List<SlotProbabilityDataSo> slotprobabilityDataSo;
    #endregion

    private void Awake()
    {
        LoadData();
    }

    protected override void LoadData()
    {
      
        //SlotProbabilityData data = JsonDataLoader.Load<SlotProbabilityData>(textAsset);

        for(int i = 0; i < textAssets.Count; i++)
        {
            // 共通のJSON読み込み機能を利用
            SlotProbabilityData data = JsonDataLoader.Load<SlotProbabilityData>(textAssets[i]);

            //読み込みに成功した場合
            if (data != null)
            {
                slotprobabilityDataSo[i].CopyData(data);

                Debug.Log("データが読み込まれました");
            }

        }


    }
}