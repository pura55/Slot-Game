using UnityEngine;
using System.Collections;

/// <summary>
/// レベルアップUI
/// 
/// レベルアップで表示するUIを管理します
/// </summary>
public class LevelUpUI : MonoBehaviour
{
    #region Config
    [SerializeField] private GameObject levelUpText; // レベルアップテキスト
    [SerializeField] private GameObject countUpNumber; // レベルアップに応じた数字のカウントアップ

    [SerializeField] private float displayDime = 1.5f; // 表示時間
    #endregion

    private void OnEnable()
    {
        PlayerLevelManager.OnLevelUp += StartDisplayText;
    }

    private void OnDisable()
    {
        PlayerLevelManager.OnLevelUp -= StartDisplayText;
    }

    /// @brief 表示を開始します
    private void StartDisplayText()
    {
        StartCoroutine(WaitDisplayTime());
    }

    /// @brief 表示時間を待機します
    private IEnumerator WaitDisplayTime()
    {
        levelUpText.SetActive(true);
        countUpNumber.SetActive(true);

        yield return new WaitForSeconds(displayDime);

        levelUpText.SetActive(false);
        countUpNumber.SetActive(false);
    }
}
