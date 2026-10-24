using UnityEngine;
using TMPro;

/// <summary>
/// 点滅状態
/// </summary>
public enum BlinkState
{
    INCREMENT, // 増加
    DECREMENT  // 減少
}

/// <summary>
/// 残りターンを管理します
/// </summary>
public class LeftTurnManager : MonoBehaviour
{
    #region Config
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private float blinkSpeed = 1f;
    #endregion

    #region State
    private Color textColor; // 文字の色
    private const float redColor = 1.0f; // 赤色の固定値
    private const float maxBlinkAmount = 1f; // 点滅の最大値
    private float blinkColor = 0f; // 点滅している色
    private BlinkState blinkState = BlinkState.INCREMENT; // 点滅状態
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textColor = new Color(redColor, blinkColor, blinkColor);
        textMeshPro.color = textColor;
    }

    // Update is called once per frame
    void Update()
    {
        BlinkText();
    }

    /// @brief 点滅させます
    private void BlinkText()
    {
        switch (blinkState)
        { 
            case BlinkState.INCREMENT:
                IncreaseColor();
                break;
            case BlinkState.DECREMENT:
                DecreaseColor();
                break;
            default:
                break;
        }

        textColor = new Color(redColor, blinkColor, blinkColor);

        textMeshPro.color = textColor;
    }

    /// @brief 色を増加させます
    private void IncreaseColor()
    {
        blinkColor += blinkSpeed * Time.deltaTime;

        if(maxBlinkAmount <= blinkColor)
        {
            blinkColor = maxBlinkAmount;
            blinkState = BlinkState.DECREMENT;
        }
    }

    /// @brief 色を減少させます
    private void DecreaseColor()
    {
        blinkColor -= blinkSpeed * Time.deltaTime;

        // 色が0(最低値）になった場合
        if(blinkColor <= 0f)
        {
            blinkColor = 0f;
            blinkState = BlinkState.INCREMENT;
        }
    }

    /// @brief 残りターンを変更します
    public void ChangeLeftTurn(int leftTurn)
    {
        textMeshPro.text = leftTurn.ToString();
    }
}
