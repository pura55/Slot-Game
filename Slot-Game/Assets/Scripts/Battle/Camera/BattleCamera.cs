using UnityEngine;

/// <summary>
/// バトルカメラ
/// 
/// バトル用のカメラを操作します
/// </summary>
public class BattleCamera : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 0.5f;
    [SerializeField] private float movementDistance = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
    }

    private void CameraMovement()
    {
        float movementCos = Mathf.Cos(cameraSpeed * Time.time);
        Vector3 amount = new Vector3(movementCos * movementDistance, transform.position.y, transform.position.z);
        transform.position = amount;
    }
}
