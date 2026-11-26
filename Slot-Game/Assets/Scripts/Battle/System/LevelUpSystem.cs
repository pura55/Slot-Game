using UnityEngine;

/// <summary>
/// レベルアップシステム
/// 
/// レベル上げをするための処理を担当します
/// </summary>
public class LevelUpSystem : MonoBehaviour
{
    #region State
    private int requiredExp = 100; // 必要経験値
    private int increaseGainRate = 2; //　必要経験上昇倍率
    #endregion

    private void OnEnable()
    {
        PlayerLevelManager.OnExpGained += ProcessExpGain;
    }

    private void OnDisable()
    {
        PlayerLevelManager.OnExpGained -= ProcessExpGain;
    }

    /// @brief 経験値取得処理を行います
    /// @param expAmount 取得経験値量
    private void ProcessExpGain(int expAmount)
    {
        int progressExp = PlayerLevelManager.CurrentExp;
        int additionalRemainingAmount = expAmount;

        bool isProcessFinished = false;

        while(isProcessFinished)
        {
            // 次のレベル到達に必要な経験値を求めるため
            // 必要経験値から現在の経験値を減算します
            int requiredNextLevel = requiredExp - progressExp;

            if (requiredNextLevel <= additionalRemainingAmount)
            {
                additionalRemainingAmount = additionalRemainingAmount - requiredNextLevel;
                PlayerLevelManager.LevelUp();

                requiredExp *= increaseGainRate;
                progressExp = 0;
            }
            else
            {
                PlayerLevelManager.CurrentExp += additionalRemainingAmount;
                isProcessFinished = true;
            }
        }
    }
}
