using UnityEngine;
using UnityEngine.InputSystem;

public class NumberDisplayManager : MonoBehaviour
{
    [Header("各桁の数字")]
    [SerializeField] private NumberRotate hundreds;
    [SerializeField] private NumberRotate tens;
    [SerializeField] private NumberRotate ones;

    [Header("現在の数値")]
    [SerializeField, Range(0, 999)]
    private int currentNumber = 0;

    public int CurrentNumber => currentNumber;

    public bool IsRotating =>
        hundreds.IsRotating ||
        tens.IsRotating ||
        ones.IsRotating;

    void Start()
    {
        UpdateNumber(false);
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        // Spaceキーで1増加
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AddNumber();
        }
    }

    /// <summary>
    /// 数字を1増やす
    /// </summary>
    public void AddNumber()
    {
        AddValue(15);
    }

    /// <summary>
    /// 指定した数値を加算
    /// </summary>
    public void AddValue(int amount)
    {
        if (IsRotating)
            return;

        currentNumber =
            (int)(((long)currentNumber + amount) % 1000);

        if (currentNumber < 0)
            currentNumber += 1000;

        UpdateNumber(true);
    }

    /// <summary>
    /// 指定した数値に変更
    /// </summary>
    public void SetValue(int value)
    {
        if (IsRotating)
            return;

        currentNumber = Mathf.Clamp(value, 0, 999);

        UpdateNumber(true);
    }

    /// <summary>
    /// 各桁の数字を更新
    /// </summary>
    private void UpdateNumber(bool animate)
    {
        int h = currentNumber / 100;
        int t = (currentNumber / 10) % 10;
        int o = currentNumber % 10;

        if (animate)
        {
            hundreds.RotateTo(h);
            tens.RotateTo(t);
            ones.RotateTo(o);
        }
        else
        {
            hundreds.SetNumber(h);
            tens.SetNumber(t);
            ones.SetNumber(o);
        }
    }
}