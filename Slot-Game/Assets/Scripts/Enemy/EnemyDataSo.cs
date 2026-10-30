using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy_Scriptable")]
public class EnemyDataSo : ScriptableObject
{
    // エネミーのデータをリスト化する変数
    public List<EnemyDataBase> Enemys;

    /// @brief データをコピーする関数
    public void CopyData(EnemyData data)
    {
        // リストを初期化
        Enemys = new List<EnemyDataBase>();

        Debug.Log($"コピーまでは来てます");

        foreach (var Enemy in data.Enemys)
        {
            Debug.Log($"データコピー中");

            // 実体を作る
            EnemyDataBase progressEnemy = new EnemyDataBase();
            progressEnemy.ID = Enemy.ID;
            progressEnemy.HP = Enemy.HP;
            progressEnemy.XP = Enemy.XP;

            // 実体を入れる
            Enemys.Add(progressEnemy);
        }
    }
}