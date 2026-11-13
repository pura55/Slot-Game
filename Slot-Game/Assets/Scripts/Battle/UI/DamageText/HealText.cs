using UnityEngine;

/// <summary>
/// ヒールテキスト
/// 
/// 回復テキストの処理を行います
/// </summary>
public class HealText : PooledText
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = speed;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    /// @brief 移動処理を行う関数
    protected override void Movement()
    {
        float amount = velocity * Time.deltaTime;
        rectTransform.position += new Vector3(0f, amount, 0f);
    }

    /// @brief 情報を設定する関数
    public void SetInfo(Vector3 position, int healAmount)
    {
        // テキストを設定
        textMeshPro.color = Color.green;
        textMeshPro.SetText("+{0}", healAmount);

        rectTransform.position = new Vector3(position.x, position.y, position.z);
        StartCoroutine(WaitToRelease());
    }
}
