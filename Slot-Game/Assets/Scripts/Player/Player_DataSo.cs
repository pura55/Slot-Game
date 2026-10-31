using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player_Scriptable")]
public class Player_DataSo : ScriptableObject
{
    //プレイヤーのデータをリスト化する変数
    public List<Player_DataBase> Statuses;

    /// @brief データをコピーする関数
    public void CopyData(Player_Data data)
    {
        // リストを初期化
        Statuses = new List<Player_DataBase>();

        Debug.Log($"コピーまでは来てます");

        foreach (var Status in data.Statuses)
        {
            Debug.Log($"データコピー中");

            // 実体を作る
            Player_DataBase progressStatus = new Player_DataBase();
            progressStatus.Level = Status.Level;
            progressStatus.HP = Status.HP;

            // 実体を入れる
            Statuses.Add(progressStatus);
        }
    }
}
