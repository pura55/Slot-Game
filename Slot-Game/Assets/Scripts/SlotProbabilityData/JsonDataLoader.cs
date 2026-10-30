using UnityEngine;

//JSONファイルを読み込み、指定されたデータ型に変換する共通クラス

public static class JsonDataLoader
{
    public static T Load<T>(TextAsset jsonFile) where T : class
    {
        //JSONファイルが設定されているか確認
        if (jsonFile == null)
        {
            Debug.LogError("JSONファイルが設定されていません。");
            return null;
        }

        //JSONの内容を指定されたデータ型に変換
        T data = JsonUtility.FromJson<T>(jsonFile.text);


        //変換に失敗した場合のエラー出力
        if (data == null)
        {
            Debug.LogError($"JSONの読み込みに失敗しました: {jsonFile.name}");
            return null;
        }

        return data;
    }
}
