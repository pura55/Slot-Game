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
    private static int currentLevel = 1; 

    private const int maxLevel = 10; // 最大レベル
    private const int minLevel = 1; // 最少レベル

    // 現在の経験値
    public static int CurrentExp { get { return currentExp; } set { currentExp = value; } }
    private static int currentExp = 0; 

    public static event Action<int> OnExpGained; // 経験値を取得した際のイベント処理
    public static event Action OnLevelUp; // レベルアップした際のイベント処理
    #endregion

    /// @brief レベルの初期化を行います
    public static void InitLevel()
    {
        // 初期化経験値
        int initExpAmount = 0; 

        CurrentLevel = minLevel;
        CurrentExp = initExpAmount;
    }

    /// @brief レベルアップを行います
    public static void LevelUp()
    {
        CurrentLevel++;

        if(maxLevel <= CurrentLevel)
        {
            CurrentLevel = maxLevel;
        }

        OnLevelUp?.Invoke();

        Debug.Log("レベルアップ：" + $"{CurrentLevel - 1}" + "→" + $"{CurrentLevel}");
    }

    /// @brief 経験値獲得の処理を開始します
    public static void GainExp(int exp)
    {
        OnExpGained?.Invoke(exp);
    }
}
