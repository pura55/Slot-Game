using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// ダメージテキスト
/// 
/// ダメージテキストの処理を行います
/// </summary>
public class DamageText : PooledText
{
    #region Config
    [SerializeField] private float gravity = 2f; // 重力
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    /// @brief 移動処理を行う関数
    protected override void Movement()
    {
        velocity -= gravity;

        float amount = velocity * Time.deltaTime;
        rectTransform.position += new Vector3(0f, amount, 0f);
    }

    /// @brief 情報を設定する関数
    public void SetInfo(Vector3 position, int damageAmount, DamagedTarget target)
    {
        // テキストを設定
        switch (target)
        {
            case DamagedTarget.ENEMY:
                textMeshPro.color = Color.white;
                textMeshPro.text = damageAmount.ToString();
                break;
            case DamagedTarget.PLAYER:
                textMeshPro.color = Color.red;
                textMeshPro.text = "-" + damageAmount;
                break;
        }

        rectTransform.position = new Vector3(position.x, position.y, position.z);
        velocity = speed;
        StartCoroutine(WaitToRelease());
    }
}
