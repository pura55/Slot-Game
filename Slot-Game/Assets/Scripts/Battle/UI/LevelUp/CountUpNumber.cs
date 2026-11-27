using UnityEngine;
using TMPro;

/// <summary>
/// カウントアップナンバー
/// 
/// 繰り上がる数字を管理します
/// </summary>
public class CountUpNumber : MonoBehaviour
{
    #region Config
    [SerializeField] private TextMeshProUGUI textMeshProUGUI; // テキスト
    #endregion

    // Update is called once per frame
    void Update()
    {
        textMeshProUGUI.text = $"{PlayerLevelManager.CurrentLevel - 1}" + " → " + $"{PlayerLevelManager.CurrentLevel}";
    }
}
