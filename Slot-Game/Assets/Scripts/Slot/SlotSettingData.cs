using UnityEngine;

//確率データを持たせるスクリプトです

// 役の種類(None はハズレ)
public enum Role { None, Seven, Bar, Animal, Bell, Grape, Cherry, Clown }


// 1つの設定ぶんの確率(ハズレは残りで自動計算)
[System.Serializable]
public class RoleRates
{
    [Range(0f, 1f)] public float seven = 0.0030f;
    [Range(0f, 1f)] public float bar = 0.0030f;
    [Range(0f, 1f)] public float animal = 0.0100f;
    [Range(0f, 1f)] public float bell = 0.0153f;
    [Range(0f, 1f)] public float grape = 0.1526f;
    [Range(0f, 1f)] public float cherry = 0.0229f;
    [Range(0f, 1f)] public float clown = 0.0100f;

    public float Total
    {
        get { return seven + bar + animal + bell + grape + cherry + clown; }
    }

    public float NoneRate
    {
        get { return 1f - Total; }
    }
}