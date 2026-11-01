using UnityEngine;

/// <summary>
/// キャラクターの基底アニメーションクラス
/// 
/// キャラクターの共通アニメーション処理を行います
/// </summary>
public abstract class CharacterAnimation : MonoBehaviour
{
    #region Config
    [Header("RENDERER")]
    [SerializeField] protected SpriteRenderer spriteRenderer; // スプライトレンダラー
    #endregion

    #region State
    // ダメージ
    protected const float damagedRedColor = 1f; // ダメージ時の赤色
    protected const float damagedAlpha = 1f; // ダメージ時のα値
    protected float currentDamagedColor = 1f; // 現在のダメージの色
    protected float rateColorChange = 15f; // 色の変色速度
    protected bool isDamaged = false; // ダメージフラグ
    protected bool isCharacterRed = false; // キャラクターが赤くなったかどうかのフラグ
    #endregion

    /// @brief ダメージ演出を設定します
    public void SetDamagedPresentation()
    {
        isDamaged = true;
        isCharacterRed = false;
    }

    /// @brief ダメージ変出を実行します
    protected void ExecuteDamagedPresentation()
    {
        // ダメージ色
        Color damagedColor;

        // 赤色から通常色
        if (isCharacterRed)
        {
            currentDamagedColor += rateColorChange * Time.deltaTime;
            // 他の色が赤色と同等以上になったら変色を終了
            if (damagedRedColor <= currentDamagedColor)
            {
                currentDamagedColor = damagedRedColor;
                isDamaged = false;
            }
        }
        else // 通常色から赤色
        {
            currentDamagedColor -= rateColorChange * Time.deltaTime;

            // 色の最低値になった場合通常色への変化に切り替えます
            if (currentDamagedColor <= 0)
            {
                currentDamagedColor = 0;
                isCharacterRed = true;
            }
        }

        damagedColor = new Color(damagedRedColor, currentDamagedColor, currentDamagedColor, damagedAlpha);
        spriteRenderer.color = damagedColor;
    }
}
