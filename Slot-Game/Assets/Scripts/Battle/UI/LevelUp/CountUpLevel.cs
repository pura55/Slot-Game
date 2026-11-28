using UnityEngine;
using TMPro;

/// <summary>
/// カウントアップレベル
/// 
/// 繰り上がるレベルの数字を表示します
/// </summary>
public class CountUpLevel : MonoBehaviour
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
