using UnityEngine;

//エネミーデータを検索・取得するためのクラス
public class Player_DataAccessor : MonoBehaviour
{
    [SerializeField] private Player_DataSo player_Data;

    //Level = プレイヤーのレベル
    //HP = プレイヤーの体力
    public bool TryGetStatus(int Level, out int HP)
    {
        //取得失敗時の初期値の設定
        HP = 0;

        //ScriptableObjectまたはデータリストが存在しない場合は終了
        if (player_Data == null ||
            player_Data.Statuses == null)
        {
            return false;
        }


        //ID名が一致するデータを検索
        foreach (var item in player_Data.Statuses)
        {
            if (item.Level == Level)
            {
                //一致したデータを返す
                HP = item.HP;
                return true;
            }
        }

        //該当するIDが見つからなかった場合
        return false;
    }
}