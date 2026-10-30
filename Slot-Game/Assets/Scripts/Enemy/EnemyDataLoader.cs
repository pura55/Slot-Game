using UnityEngine;

public class EnemyDataLoader : MonoBehaviour
{
    #region Config
    [SerializeField] private TextAsset enemyData;
    [SerializeField] private EnemyDataSo enemyDataSo;
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
         EnemyData data = JsonDataLoader.Load<EnemyData>(enemyData);

        // 読み込みに成功した場合
        if (data != null)
        {
            enemyDataSo.CopyData(data);

            Debug.Log("データが読み込まれました。");
        }
    }
}
