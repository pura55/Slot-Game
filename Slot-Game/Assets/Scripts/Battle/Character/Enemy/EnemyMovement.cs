using UnityEngine;

/// <summary>
/// エネミームーブメント
/// 
/// 敵の移動処理を行います
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    #region Config
    [SerializeField] private float movementSpeed = 2f; // 移動速度
    [SerializeField] private float movementDistance = 4f; // 移動距離
    #endregion

    #region State
    private bool isMove = false;
    private float movementDistanceCount; // 移動距離のカウント
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMove)
        {
            return;
        }

        ApplyChange();
    }

    /// @brief 移動を指示します
    public void OperateMovement()
    {
        isMove = true;
    }

    /// @brief 処理を反映します
    private void ApplyChange()
    {
        if (movementDistance <= movementDistanceCount)
        {
            isMove = false;
        }

        Vector3 amount = new Vector3(movementSpeed * Time.deltaTime, 0, 0);
        movementDistanceCount += amount.x;
        transform.position -= amount;
    }
}
