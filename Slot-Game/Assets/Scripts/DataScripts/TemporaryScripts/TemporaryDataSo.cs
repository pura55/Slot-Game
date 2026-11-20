using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Temporary_Scriptable")]
public class TemporaryDataSo : ScriptableObject
{
    //仮置きのデータをリスト化する変数
    public List<TemporaryDataBase> Temporarys;

    //データをコピーする関数
    public void CopyData(TemporaryData data)
    {
        //リストを初期化
        Temporarys = new List<TemporaryDataBase>();

        Debug.Log($"コピーまでは来てます");

        foreach (var Temporary in data.Temporarys )
        {
            Debug.Log($"データコピー中");

            //実体を作る
            TemporaryDataBase progressTemporary = new TemporaryDataBase();
            progressTemporary.ID = Temporary.ID;
            progressTemporary.HitPoint = Temporary.HitPoint;
            progressTemporary.Power = Temporary.Power;

            //実体を入れる
            Temporarys.Add( progressTemporary );
        }
    }
}
