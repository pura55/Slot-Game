using UnityEngine;

//ATカウントの数値を検索・取得するためのクラス
public class ATCount_DataAccessor : MonoBehaviour
{
    [SerializeField] private ATCount_DataSo atcount_data;

    //Setting = スロットの設定
    //Min = ATに入るまでの最小値
    //Max = ATに入るまでの最大値
    public bool TryGetATCount(int Setting, out int Min, out int Max)
    {
        //取得失敗時の初期値の設定
        Min = 0;
        Max = 0;


        //ScriptableObjectまたはデータリストが存在しない場合は終了
        if (atcount_data == null || atcount_data.ATCount == null)
        {
            return false;
        }


        //ID名が一致するデータを検索
        foreach (var item in atcount_data.ATCount)
        {
            if (item.Setting == Setting)
            {
                //一致したデータを返す
                Min = item.Min; 
                Max = item.Max;
                return true;
            }
        }

        //該当するIDが見つからなかった場合
        return false;
    }
}
