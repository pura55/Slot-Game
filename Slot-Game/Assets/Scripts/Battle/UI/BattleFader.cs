using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// バトルフェーダー
/// 
/// バトルのフェーダー
/// </summary>
public class BattleFader : MonoBehaviour
{
    #region Config
    [SerializeField] private Image fade; // フェード
    #endregion

    #region State
    private float maxFadeAlpha = 1f; // フェードの最大値
    private float fadingSpeed = 1f; // フェード速度
    private float fadeAlpha = 0f; // フェードのα値
    private float fadeColor = 1f;
    private bool isFadeIn = false;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FadeIn();
    }

    /// @brief フェードイン処理を申請します
    public void OrderFadeIn()
    {
        isFadeIn = true;
    }

    /// @brief フェードインを行います
    private void FadeIn()
    {
        if(maxFadeAlpha <= fadeAlpha && isFadeIn)
        {
            return;
        }
        fadeAlpha += fadingSpeed * Time.deltaTime;
        fade.color = new Color(fadeColor, fadeColor, fadeColor, fadeAlpha);
    }
}
