using UnityEngine;

/// <summary>
/// エネミーステータス
/// 
/// 敵のステータスを保持します
/// </summary>
public class EnemyStatus : MonoBehaviour
{
    #region Config
    [SerializeField] private int enemyId = 1; // ID
    #endregion

    #region State
    // 体力
    public int EnemyHp { get { return enemyHp; } }
    private int enemyHp = 50;

    // 経験値
    public int EnemyExp { get { return experiencePoints; } }
    private int experiencePoints = 60;
    #endregion
}
