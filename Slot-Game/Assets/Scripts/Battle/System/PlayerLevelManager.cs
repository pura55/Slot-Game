using UnityEngine;

/// <summary>
/// プレイヤーレベルマネージャー
/// 
/// プレイヤーのレベルを管理します
/// </summary>
public static class PlayerLevelManager
{
    #region State
    public static int CurrentLevel { get { return currentLevel; } private set { currentLevel = value;  } }
    private static int currentLevel = 0;
    #endregion

    /// @brief レベルの初期化を行います
    public static void InitLevel()
    {
        currentLevel = 0;
    }
}
