using UnityEngine;

/// <summary>
/// エネミーステータス
/// 
/// 敵のステータスを保持します
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    #region Config
    [Header("PlayerComponents")]
    [SerializeField] private Player_DataAccessor dataAccessor; // データアクセサー
    [SerializeField] private PlayerHealth playerHealth; // プレイヤーヘルス

    [Header("DamageCommandEffect")]
    [SerializeField] private DamageEffect heavyDamage; // 強ダメージ
    [SerializeField] private DamageEffect lightDamage; // 弱ダメージ
    #endregion

    #region State
    // プレイヤーの体力（現状の体力ではなく最大値）
    public int PlayerHp { get { return playerHp; } }
    private int playerHp = 50; 

    private int heavyAttackDamage = 50; // ダメージ量（強）
    private int lightAttackDamage = 15; // ダメージ量（弱）
    #endregion

    private void OnEnable()
    {
        PlayerLevelManager.OnLevelUp += UpdateStatus;
    }

    private void OnDisable()
    {
        PlayerLevelManager.OnLevelUp -= UpdateStatus;
    }

    private void Start()
    {
        InitStatus();
    }

    /// @brief ステータスの初期化を行います
    private void InitStatus()
    {
        UpdateStatus();
        playerHealth.ResetHealth();
    }

    /// @brief ステータスの更新を行います
    private void UpdateStatus()
    {
        dataAccessor.TryGetStatus(PlayerLevelManager.CurrentLevel, out playerHp, out lightAttackDamage, out heavyAttackDamage);
        UpdateDamageAmount();
    }

    /// @brief ダメージ量の更新を行います
    private void UpdateDamageAmount()
    {
        heavyDamage.DamageAmount = heavyAttackDamage;
        lightDamage.DamageAmount = lightAttackDamage;
    }
}
