using UnityEngine;

public class Player_DataLoader : DataLoaderBase
{
    #region Config
    [SerializeField] private Player_DataSo player_DataSo;
    #endregion

    private void Awake()
    {
        LoadData();
    }

    protected override void LoadData()
    {
        // 共通のJSON読み込み機能を利用
        Player_Data data = JsonDataLoader.Load<Player_Data>(textAsset);

        // 読み込みに成功した場合
        if (data != null)
        {
            player_DataSo.CopyData(data);

            Debug.Log("データが読み込まれました。");
        }
    }
}
