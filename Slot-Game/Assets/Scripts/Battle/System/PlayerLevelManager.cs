using System;
using UnityEngine;

/// <summary>
/// プレイヤーレベルマネージャー
/// 
/// プレイヤーのレベルを管理します
/// </summary>
public static class PlayerLevelManager
{
    #region State
    // 現在のレベル
    public static int CurrentLevel { get { return currentLevel; } private set { currentLevel = value;  } }
    private static int currentLevel = 0; 

    private const int maxLevel = 10; // 最大レベル

    // 現在の経験値
    public static int CurrentExp { get { return currentExp; } set { currentExp = value; } }
    private static int currentExp = 0; 

    public static event Action<int> OnExpGained; // 経験値を取得した際のイベント処理
    #endregion

    /// @brief レベルの初期化を行います
    public static void InitLevel()
    {
        // 初期化量のレベルは0
        int initAmount = 0; 

        CurrentLevel = initAmount;
        CurrentExp = initAmount;
    }

    /// @brief レベルアップを行います
    public static void LevelUp()
    {
        CurrentLevel++;

        if(maxLevel <= CurrentLevel)
        {
            CurrentLevel = maxLevel;
        }

        Debug.Log("レベルアップ：" + $"{CurrentLevel - 1}" + "→" + $"{CurrentLevel}");
    }

    /// @brief 経験値獲得の処理を開始します
    public static void GainExp(int exp)
    {
        OnExpGained?.Invoke(exp);
    }
}
