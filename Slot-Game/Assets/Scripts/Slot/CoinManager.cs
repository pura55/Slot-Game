using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// CoinManager
/// 
/// コインを管理するスクリプト
/// </summary>
public class CoinManager : MonoBehaviour
{
    [Header("コイン枚数")]
    [SerializeField] private int coinsOwned = 0; // コイン保有数

    [Header("TMP")]
    [SerializeField] private TMP_Text numberText;

    // すでに使用したか
    private bool isUsed = false;
    public bool IsUsed => isUsed;

    void Start()
    {
        AddGold();
        UpdateText();
    }

    private void Update()
    {
        if (!DebugManager.IsDebug)
        {
            return;
        }

        DebugInput();
    }

    /// @brief コインを追加します
    public void AddGold()
    {
        ChangeCoinsNumber(100);
        UpdateText();
    }

    // ボタンで3枚消費（1回だけ）
    public void UseNumber()
    {
        // すでに消費していたら何もしない
        if (isUsed)
            return;

        // 3枚未満なら消費できない
        if (coinsOwned < 3)
        {
            Debug.Log("枚数が足りません");
            return;
        }

        // 3枚消費
        ChangeCoinsNumber(-3);
        isUsed = true;

        UpdateText();

        Debug.Log("3枚消費しました");
    }

    void UpdateText()
    {
        if (numberText == null)
            return;

        numberText.text = $"{coinsOwned}";
    }

    /// @brief コインの枚数を変更します
    private void ChangeCoinsNumber(int amount)
    {
        coinsOwned += amount;
    }

    /// @brief デバックインプットを行います。
    private void DebugInput()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            AddGold();
        }
    }
}

