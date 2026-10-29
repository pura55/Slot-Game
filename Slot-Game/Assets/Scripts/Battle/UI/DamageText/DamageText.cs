using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// ダメージテキスト
/// 
/// ダメージテキストの処理を行います
/// </summary>
public class DamageText : APooledObject
{
    #region Config
    [SerializeField] private float lifeTime = 1f; // ライフタイム
    [SerializeField] private float speed = 10f; // 設定上の速度
    [SerializeField] private float gravity = 2f; // 重力
    [SerializeField] private TMP_Text textMeshPro; // ダメージのテキスト
    [SerializeField] private RectTransform rectTransform; // トランスフォーム
    #endregion

    #region State
    private float velocity = 3f; // 移動速度
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    /// @brief 移動処理を行う関数
    private void Movement()
    {
        velocity -= gravity;

        float amount = velocity * Time.deltaTime;
        rectTransform.position += new Vector3(0f, amount, 0f);
    }

    /// @brief 情報を設定する関数
    public void SetInfo(Vector3 position, int damage, DamagedTarget target)
    {
        // テキストを設定
        switch (target)
        {
            case DamagedTarget.ENEMY:
                textMeshPro.color = Color.white;
                textMeshPro.text = damage.ToString();
                break;
            case DamagedTarget.PLAYER:
                textMeshPro.color = Color.red;
                textMeshPro.text = "-" + damage;
                break;
        }

        rectTransform.position = new Vector3(position.x, position.y, position.z);
        velocity = speed;
        StartCoroutine(WaitToRelease());
    }

    /// @brief リリースを待機する関数
    private IEnumerator WaitToRelease()
    {
        // リリース待機
        yield return new WaitForSeconds(lifeTime);

        Release();
    }
}
