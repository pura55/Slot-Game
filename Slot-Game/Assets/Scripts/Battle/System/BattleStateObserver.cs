/// <summary>
/// キャラクターバトルステート
///
/// キャラクターのバトル状態
/// </summary>
public enum CharacterBattleState
{
    IDLE,
    ACTION
}

/// <summary>
/// バトルステートオブザーバー
///
/// キャラクターのバトル時の状態観測を行います
/// </summary>
public static class BattleStateObserver
{
    #region State
    private static CharacterBattleState playerBattleState = CharacterBattleState.IDLE; // プレイヤーの状態
    private static CharacterBattleState enemyBattleState = CharacterBattleState.IDLE; // 敵の状態
    public static CharacterBattleState PlayerBattleState { get { return playerBattleState; } set { playerBattleState = value; } }
    public static CharacterBattleState EnemyBattleState { get { return enemyBattleState; } set { enemyBattleState = value; } }
    #endregion
}
