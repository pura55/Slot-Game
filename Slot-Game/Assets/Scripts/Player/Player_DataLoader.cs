using UnityEngine;

public class Player_DataLoader : MonoBehaviour
{
    #region Config
    [SerializeField] private TextAsset player_Data;
    [SerializeField] private Player_DataSo player_DataSo;
    #endregion

    private void Awake()
    {
        LoadData();
    }

    private void Start()
    {

    }

    private void LoadData()
    {
        // 共通のJSON読み込み機能を利用
        Player_Data data = JsonDataLoader.Load<Player_Data>(player_Data);

        // 読み込みに成功した場合
        if (data != null)
        {
            player_DataSo.CopyData(data);

            Debug.Log("データが読み込まれました。");
        }
    }
}
