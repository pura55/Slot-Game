using UnityEngine;

//スロット確率データを検索・取得するためのクラス
public class SlotProbabilityDataAccessor : MonoBehaviour
{
    [SerializeField]private SlotProbabilityDataSo slotData;

    //指定されたシンボル名に対応する確率を取得
    //symbolName = 取得したいシンボルの名前
    //probability = 取得した確率　該当データがない場合は0
    public bool TryGetProbability(string symbolName,out float probability_1, out float probability_2, out float probability_3, out float probability_4, out float probability_5, out float probability_6)          
    {
        //取得失敗時の初期値の設定
        probability_1 = 0f;
        probability_2 = 0f;
        probability_3 = 0f;
        probability_4 = 0f;
        probability_5 = 0f;
        probability_6 = 0f;

        //ScriptableObjectまたはデータリストが存在しない場合は終了
        if (slotData == null ||
            slotData.SlotProbabilities == null)
        {
            return false;
        }


        //シンボル名が一致するデータを検索
        foreach (var item in slotData.SlotProbabilities)
        {
            if (item.symbolname == symbolName)
            {
                //一致したデータの確率を返す
                probability_1 = item.probability_1;
                probability_2 = item.probability_2;
                probability_3 = item.probability_3;
                probability_4 = item.probability_4;
                probability_5 = item.probability_5;
                probability_6 = item.probability_6;
                return true;
            }
        }

        //該当するシンボルが見つからなかった場合
        return false;
    }
}