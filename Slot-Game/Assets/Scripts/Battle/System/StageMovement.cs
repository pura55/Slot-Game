using UnityEngine;

/// <summary>
/// ステージムーブメント
/// 
/// ステージの移動処理を行います
/// </summary>
public class StageMovement : MonoBehaviour
{
    #region Config 
    [SerializeField] private float moveSpeed = 1.0f; // 移動速度
    #endregion

    #region
    // トランスフォームを返します
    public Transform StageTransfrom{ get{return gameObject.transform;} } 

    private const float stopMovement = 0f; // 移動停止
    private float currentVelocity = 0f; // 現在の速度
    private Vector3 firstPosition = new Vector3();
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyChange();
    }

    /// @brief スピードを設定します
    public void SetSpeed(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.MOVE:
                currentVelocity = moveSpeed;
                break;
            case PlayerState.BATTLE:
                currentVelocity = stopMovement;
                break;
            default:
                currentVelocity = stopMovement;
                break;
        }
    }

    /// @brief 処理を反映します
    private void ApplyChange()
    {
        Vector3 amount = new Vector3(currentVelocity * Time.deltaTime, 0, 0);
        transform.position -= amount;
    }
     
    /// @brief 初期位置へ座標を戻します
    public void SetFirstPosition()
    {
        transform.position = firstPosition;
    }
}
