using UnityEngine;

public class ATCount_DataLoader : DataLoaderBase
{
    #region Config
    [SerializeField] private ATCount_DataSo atcount_DataSo;
    #endregion

    private void Awake()
    {
        LoadData();
    }

    protected override void LoadData()
    {
        // 共通のJSON読み込み機能を利用
        ATCount_Data data = JsonDataLoader.Load<ATCount_Data>(textAsset);

        //読み込みに成功した場合
        if (data != null)
        {
            atcount_DataSo.CopyData(data);

            Debug.Log("データが読み込まれました。");
        }
    }
}

