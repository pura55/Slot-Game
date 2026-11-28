using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ATCount_Scriptable")]
public class ATCount_DataSo : ScriptableObject
{
    //ATカウントのデータをリスト化する変数
    public List<ATCount_DataBase> ATCount;

    /// @brief データをコピーする関数
    public void CopyData(ATCount_Data data)
    {
        //リストを初期化
        ATCount = new List<ATCount_DataBase>();

        Debug.Log($"コピーまでは来てます");

        foreach (var atcount in data.ATCount )
        {
            Debug.Log($"データコピー中");

            //実体を作る
            ATCount_DataBase progressatcount = new ATCount_DataBase();
            progressatcount.Setting = atcount.Setting;
            progressatcount.Min = atcount.Min;
            progressatcount.Max = atcount.Max;

            //実体を入れる
            ATCount.Add( progressatcount );
        }
    }
}