using UnityEngine;
using System.Collections;

/// <summary>
/// スポーンVFXコントローラー
/// 
/// スポーンのエフェクトを操作します
/// </summary>
public class SpawnVFXController : MonoBehaviour
{
    #region Config
    [SerializeField] private float destroyTime = 6f;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitDestroyVFX());
    }

    /// @brief エフェクトを削除するまでの待機
    IEnumerator WaitDestroyVFX()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }
}
