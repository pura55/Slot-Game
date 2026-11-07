using UnityEngine;

//スロット確率データを検索・取得するためのクラス
public class SlotProbabilityDataAccessor : MonoBehaviour
{
    [SerializeField]private SlotProbabilityDataSo slotData;

    //指定されたシンボル名に対応する確率を取得
    //symbolName = 取得したいシンボルの名前
    //probability = 取得した確率　該当データがない場合は0
    public bool TryGetProbability(string symbolName,out float probability)          
    {
        //取得失敗時の初期値の設定
        probability = 0f;

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
                probability = item.probability;
                return true;
            }
        }

        //該当するシンボルが見つからなかった場合
        return false;
    }
}