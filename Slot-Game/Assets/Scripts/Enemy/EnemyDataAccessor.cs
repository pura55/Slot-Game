using UnityEngine;

//エネミーデータを検索・取得するためのクラス
public class EnemyDataAccessor : MonoBehaviour
{
    [SerializeField] private EnemyDataSo enemyData;

    //指定されたシンボル名に対応する確率を取得
    //ID = 取得したいエネミーの名前
    //HP = エネミーの体力
    //XP = エネミーの経験値
    public bool TryGetEnemy(int ID, out int HP, out int XP)
    {
        //取得失敗時の初期値の設定
        HP = 0;
        XP = 0;

        //ScriptableObjectまたはデータリストが存在しない場合は終了
        if (enemyData == null ||
            enemyData.Enemys == null)
        {
            return false;
        }


        //ID名が一致するデータを検索
        foreach (var item in enemyData.Enemys)
        {
            if (item.ID == ID)
            {
                //一致したデータの確率を返す
                HP = item.HP;
                XP = item.XP;
                return true;
            }
        }

        //該当するIDが見つからなかった場合
        return false;
    }
}