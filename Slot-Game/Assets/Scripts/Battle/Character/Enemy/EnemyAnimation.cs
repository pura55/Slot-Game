using UnityEngine;

/// <summary>
/// エネミーアニメーション
/// 
/// 敵のアニメーション処理を行います
/// </summary>
public class EnemyAnimation : CharacterAnimation
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamaged)
        {
            ExecuteDamagedPresentation();
        }
    }
}
