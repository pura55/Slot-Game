using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy_Scriptable")]
public class Enemy_DataSo : ScriptableObject
{
    // エネミーのデータをリスト化する変数
    public List<Enemy_DataBase> Enemys;

    /// @brief データをコピーする関数
    public void CopyData(Enemy_Data data)
    {
        // リストを初期化
        Enemys = new List<Enemy_DataBase>();

        Debug.Log($"コピーまでは来てます");

        foreach (var Enemy in data.Enemys)
        {
            Debug.Log($"データコピー中");

            // 実体を作る
            Enemy_DataBase progressEnemy = new Enemy_DataBase();
            progressEnemy.ID = Enemy.ID;
            progressEnemy.HP = Enemy.HP;
            progressEnemy.XP = Enemy.XP;

            // 実体を入れる
            Enemys.Add(progressEnemy);
        }
    }
}