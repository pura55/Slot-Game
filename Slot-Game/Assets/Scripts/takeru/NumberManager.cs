using UnityEngine;
using TMPro;

public class NumberManager : MonoBehaviour
{
    [Header("êîíl")]
    [SerializeField] private int x = 0;

    [Header("TMP")]
    [SerializeField] private TMP_Text numberText;

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

    void UpdateText()
    {
        if (numberText == null)
            return;

        numberText.text = x.ToString();
    }
}