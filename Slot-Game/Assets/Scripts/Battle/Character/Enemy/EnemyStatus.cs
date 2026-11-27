using UnityEngine;

/// <summary>
/// エネミーステータス
/// 
/// 敵のステータスを保持します
/// </summary>
public class EnemyStatus : MonoBehaviour
{
    #region Config
    [Header("ID")]
    [SerializeField] private int enemyId = 1; // ID

    [Header("Components")]
    [SerializeField] private Enemy_DataAccessor dataAccessor; // データアクセサー
    #endregion

    #region State
    // 体力
    public int EnemyHp { get { return enemyHp; } }
    private int enemyHp = 50;

    // 経験値
    public int EnemyExp { get { return experiencePoints; } }
    private int experiencePoints = 60;
    #endregion

    private void Awake()
    {
        if(dataAccessor.TryGetEnemy(enemyId, out enemyHp,out experiencePoints))
        {
            Debug.Log("敵データの取得成功");
        }
    }
}
