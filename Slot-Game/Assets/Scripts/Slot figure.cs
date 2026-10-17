
using UnityEngine;
using TMPro;

//スロットの数字に関するスクリプトです

public class SlotCreditManager : MonoBehaviour
{
    // クレジット表示用のテキスト
    #region Config
    [SerializeField] private TMP_Text creditText;
    #endregion

    // 現在のクレジット枚数
    #region State
    [SerializeField] private int credit = 0;
    #endregion

    private void Start()
    {
        UpdateCreditText();
    }


    // ボタンを押したときに呼び出す処理
    public void OnCreditButtonPressed()
    {
        if (credit <= 2)
        {
            // 2枚以下なら50枚増やす
            credit += 50;
        }
        else
        {
            // 3枚以上なら3枚減らす
            credit -= 3;
        }

        // 画面の表示を更新
        UpdateCreditText();

        Debug.Log("現在のクレジット：" + credit);
    }

    

    private void UpdateCreditText()
    {
        if (creditText != null)
        {
            creditText.text = "CREDIT: " + credit;
        }
    }
}

