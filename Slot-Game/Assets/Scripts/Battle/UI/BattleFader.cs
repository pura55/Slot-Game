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
    private const float maxFadeAlpha = 1f; // フェードの最大値
    private const float minFadeAlpha = 0f; // フェードの最小値
    private float fadingSpeed = 1f; // フェード速度
    private float fadeAlpha = 0f; // フェードのα値
    private float fadeColor = 0f;
    private bool isFadeIn = false;
    private bool isFadeOut = false;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FadeIn();
        FadeOut();
    }

    /// @brief フェードイン処理を申請します
    public void OrderFadeIn()
    {
        isFadeIn = true;
    }

    public void OrderFadeOut()
    {
        isFadeOut = true;
    }

    /// @brief フェードインを行います
    private void FadeIn()
    {
        if(!isFadeIn)
        {
            return;
        }

        fadeAlpha += fadingSpeed * Time.deltaTime;

        if(maxFadeAlpha <= fadeAlpha)
        {
            fadeAlpha = maxFadeAlpha;
            isFadeIn = false;
        }

        fade.color = new Color(fadeColor, fadeColor, fadeColor, fadeAlpha);
    }

    /// @brief フェードアウトを行います
    private void FadeOut()
    {
        if (!isFadeOut)
        {
            return;
        }

        fadeAlpha -= fadingSpeed * Time.deltaTime;

        if (fadeAlpha <= minFadeAlpha)
        {
            fadeAlpha = minFadeAlpha;
            isFadeOut = false;
        }

        fade.color = new Color(fadeColor, fadeColor, fadeColor, fadeAlpha);
    }
}
