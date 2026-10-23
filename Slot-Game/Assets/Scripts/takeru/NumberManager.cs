using UnityEngine;
using TMPro;

/// <summary>
/// NumberManager
/// 
///　コインを管理するスクリプト
/// </summary>
public class NumberManager : MonoBehaviour
{
    [Header("数値")]
    [SerializeField] private int x = 0;

    [Header("TMP")]
    [SerializeField] private TMP_Text numberText;

    // すでに使用したか
    private bool isUsed = false;
    public bool IsUsed => isUsed;

    void Start()
    {
        UpdateText();
    }

    public void AddNumber()
    {
        x += 1000;
        UpdateText();
    }

    public void AddGold()
    {
        x += 100;
        UpdateText();
    }

    // ボタンで3枚消費（1回だけ）
    public void UseNumber()
    {
        // すでに消費していたら何もしない
        if (isUsed)
            return;

        // 3枚未満なら消費できない
        if (x < 3)
        {
            Debug.Log("枚数が足りません");
            return;
        }

        // 3枚消費
        x -= 3;
        isUsed = true;

        UpdateText();

        Debug.Log("3枚消費しました");
    }


    void UpdateText()
    {
        if (numberText == null)
            return;

        numberText.text = x.ToString();
    }
}

