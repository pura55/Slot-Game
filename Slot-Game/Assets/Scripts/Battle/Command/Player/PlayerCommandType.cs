/// <summary>
/// プレイヤーコマンドタイプ
/// 
/// コマンドの種類を記載します
/// </summary>
public enum PlayerCommandType
{ 
    NONE,            // 無し
    LIGHT_ATTACK,    // 弱攻撃
    HEAVY_ATTACK,    // 強攻撃
    ADDITIONAL_TURN, // 追加ターン
    HEAL,            // 回復
    BONUS,           // ボーナス
    COIN             // コイン
}

/// プレイヤーコマンドを呼ぶ例
///
/// 「イベント宣言」
/// public event Action<PlayerCommandType> OnHit;
/// 
/// 「イベントの呼び出し」
/// 当たっていないとき：OnHit?.(PlayerCommand.None);
/// 小当たり：OnHit?,(PlayerCommand.LIGHT_ATTACK);
///