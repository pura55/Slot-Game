using UnityEngine;

public class DataLoader : MonoBehaviour
{
    #region Config
    [SerializeField] private TextAsset temporaryData;
    [SerializeField] private TemporaryDataSo temporaryDataSo;
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
        //アセットがnullではない場合に実行
        if (temporaryData != null)
        {
            //クラスに変換
            TemporaryData data = JsonUtility.FromJson<TemporaryData>(temporaryData.text);

            temporaryDataSo.CopyData(data);

            Debug.Log("データが読み込まれました。");
        }
        else
        {
            //エラーを出力
            Debug.LogError("JSONファイルの読み込みに失敗しました。");
        }
    }

}
