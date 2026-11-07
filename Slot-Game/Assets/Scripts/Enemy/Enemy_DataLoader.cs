using UnityEngine;

public class Enemy_DataLoader : MonoBehaviour
{
    #region Config
    [SerializeField] private TextAsset enemy_Data;
    [SerializeField] private Enemy_DataSo enemy_DataSo;
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
         Enemy_Data data = JsonDataLoader.Load<Enemy_Data>(enemy_Data);

        // 読み込みに成功した場合
        if (data != null)
        {
            enemy_DataSo.CopyData(data);

            Debug.Log("データが読み込まれました。");
        }
    }
}
